using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.PatientsDTOs;
using Clinic.Core.Repositories.Interfaces;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Clinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientsController(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }




        [HttpGet("ReadAllPatients")]
        public async Task<IActionResult> ReadAllPatientsAsync()
        {
            try
            {
                var patientsFromRepository = await _patientRepository.ReadAllPatientsAsync();
                var patients = _mapper.Map<IEnumerable<PatientGetDto>>(patientsFromRepository);

                return Ok(patients);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadAllPatientsAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpGet("ReadPatientById/{patientId}", Name = "ReadPatientByIdAsync")]
        public async Task<IActionResult> ReadPatientByIdAsync(Guid patientId)
        {
            try
            {
                var patientFromRepository = await _patientRepository.ReadPatientByPatientIdAsync(patientId);

                if (patientFromRepository == null)
                    return BadRequest("A patient with this id can not be found.");

                var patient = _mapper.Map<PatientGetDto>(patientFromRepository);


                return Ok(patient);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadPatientByPatientIdAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatientAsync(PatientCreateDto patient)
        {
            try
            {
                if (patient == null)
                    return BadRequest("Patient can not be null.");

                if (_patientRepository.IsNationalCodeExistsAsync(patient.NationalCode).Result)
                {
                    var message = $"A patient with this {patient.NationalCode} national code is already exists.";
                    return BadRequest(message);
                }

                var patientForRepository = _mapper.Map<Patient>(patient);

                await _patientRepository.AddPatientAsync(patientForRepository);

                return CreatedAtAction("ReadPatientById", new {id = patientForRepository.PatientId}, patient);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in CreatePatientAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPut("UpdatePatient/{patientId}")]
        public async Task<IActionResult> UpdatePatientAsync(Guid patientId, PatientUpdateDto patient)
        {
            try
            {

                var patientToUpdate = _patientRepository.ReadPatientByPatientIdAsync(patientId).Result;

                if (patientToUpdate == null)
                    return BadRequest("A patient with this id can not be found");

                if (_patientRepository.IsNationalCodeExistsAsync(patient.NationalCode).Result && patient.NationalCode != patientToUpdate.NationalCode)
                {
                    var message = $"A patient with this {patient.NationalCode} national code is already exists.";
                    return BadRequest(message);
                }

                var updatedPatient = _mapper.Map(patient, patientToUpdate);

                await _patientRepository.UpdatePatientAsync(updatedPatient);

                return Ok("Patient has been updated.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in UpdatePatientAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPatch("PartialPatientUpdate/{patientId}")]
        public async Task<IActionResult> PartialPatientUpdateAsync(Guid patientId, JsonPatchDocument<PatientUpdateDto> patchDoc)
        {
            try
            {
                var patientFromRepository = _patientRepository.ReadPatientByPatientIdAsync(patientId).Result;

                if (patientFromRepository == null)
                    return NotFound();

                #region Validations

                var patientToPatch = _mapper.Map<PatientUpdateDto>(patientFromRepository);
                patchDoc.ApplyTo(patientToPatch, ModelState);
                if (!TryValidateModel(patientToPatch))
                    return ValidationProblem(ModelState);

                if (_patientRepository.IsNationalCodeExistsAsync(patientToPatch.NationalCode).Result && patientToPatch.NationalCode != patientFromRepository.NationalCode)
                {
                    var message = $"A patient with this {patientToPatch.NationalCode} national code is already exists.";
                    ModelState.AddModelError("", message);
                    return ValidationProblem(ModelState);
                }
                #endregion


                var updatedPatient = _mapper.Map(patientToPatch, patientFromRepository);

                await _patientRepository.UpdatePatientAsync(updatedPatient);

                return Ok("Patient has been updated.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in PartialPatientUpdateAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpDelete("DeletePatient/{patientId}")]
        public async Task<IActionResult> DeletePatientAsync(Guid patientId)
        {
            try
            {
                var patientToDelete = _patientRepository.ReadPatientByPatientIdAsync(patientId).Result;

                if (patientToDelete == null)
                    return BadRequest("A patient with this id can not be found.");

                await _patientRepository.DeletePatientAsync(patientToDelete);

                return Ok("Patient has been deleted.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in DeletePatientAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }

    }
}
