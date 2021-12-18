using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.PatientsDTOs;
using Clinic.Core.DTOs.VisitsDTOs;
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
    public class VisitsController : ControllerBase
    {
        private readonly IVisitRepository _visitsRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public VisitsController(IVisitRepository visitsRepository, IPatientRepository patientRepository, IMapper mapper)
        {
            _visitsRepository = visitsRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

    


        [HttpGet("ReadAllVisits")]
        public async Task<IActionResult> ReadAllVisitsAsync()
        {
            try
            {
                var visitListFromRepository = await _visitsRepository.ReadAllVisitsAsync();

            
                var visitListForShow = new List<VisitGetDto>();

                foreach (var item in visitListFromRepository)
                {
                    var visit = _mapper.Map<VisitGetDto>(item);
                    visit.PatientName = _patientRepository.ReadPatientByPatientIdAsync(item.PatientId).Result.Name;
                    visitListForShow.Add(visit);
                }

                return Ok(visitListForShow);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadAllVisitsAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }



        [Route("ReadAllVisitsOfPatientByPatientId/{patientId}")]
        [HttpGet()]
        public async Task<IActionResult> ReadAllVisitsOfPatientByPatientIdAsync(Guid patientId)
        {
            try
            {
                var visitsFromRepository = _visitsRepository.ReadAllVisitsOfPatientByPatientIdAsync(patientId);
                var patient = await _patientRepository.ReadPatientByPatientIdAsync(patientId);

                if (patient == null)
                    return BadRequest("A patient with this id can not be found");

                var visits = _mapper.Map<IEnumerable<VisitGetDto>>(visitsFromRepository);

                if (!visits.Any())
                    return Ok("There is no visits for this patient.");

                foreach (var visit in visits)
                {
                    visit.PatientName = patient.Name;
                }

                return Ok(visits);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadAllVisitsOfPatientByPatientIdAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpGet("ReadVisitById/{visitId}", Name = "ReadVisitByIdAsync")]
        public async Task<IActionResult> ReadVisitByIdAsync(Guid visitId)
        {
            try
            {
                var visitFromRepository = await _visitsRepository.ReadVisitByVisitIdAsync(visitId);

                if (visitFromRepository == null)
                    return BadRequest("A visit with this id can not be found");

                var visitForShow = _mapper.Map<VisitGetDto>(visitFromRepository);
                visitForShow.PatientName = _patientRepository.ReadPatientByPatientIdAsync(visitFromRepository.PatientId).Result.Name;

                return Ok(visitForShow);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in ReadVisitByVisitIdAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPost("CreateVisit")]
        public async Task<IActionResult> CreateVisitAsync(VisitCreateDto visit)
        {
            try
            {
                if (visit == null)
                    return BadRequest("Visit can not be found");

                if (_patientRepository.ReadPatientByPatientIdAsync(visit.PatientId) == null)
                    return BadRequest("Patient with this id can not be found.");

                var visitForRepository = _mapper.Map<Visit>(visit);

                await _visitsRepository.AddVisitAsync(visitForRepository);

                return CreatedAtAction("ReadVisitById", new { visitForRepository.VisitId }, visit);
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in CreateVisitAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPut("UpdateVisit/{visitId}")]
        public async Task<IActionResult> UpdateVisitAsync(Guid visitId, VisitUpdateDto visit)
        {
            try
            {
                var visitFromRepository = _visitsRepository.ReadVisitByVisitIdAsync(visitId).Result;

                if (visitFromRepository == null)
                    return BadRequest("A visit with this id can not be found");

                var updatedVisit = _mapper.Map(visit, visitFromRepository);

                await _visitsRepository.UpdateVisitAsync(updatedVisit);

                return Ok("Visit has been updated.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in UpdateVisitAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpPatch("PartialVisitUpdate/{visitId}")]
        public async Task<IActionResult> PartialVisitUpdateAsync(Guid visitId, JsonPatchDocument<VisitUpdateDto> patchDoc)
        {
            try
            {
                var visitFromRepository = _visitsRepository.ReadVisitByVisitIdAsync(visitId).Result;

                if (visitFromRepository == null)
                    return BadRequest("A visit with this id can not be found");

                #region Validations

                var visitToPatch = _mapper.Map<VisitUpdateDto>(visitFromRepository);
                patchDoc.ApplyTo(visitToPatch, ModelState);
                if (!TryValidateModel(visitToPatch))
                    return ValidationProblem(ModelState);

                #endregion


                var updatedVisit = _mapper.Map(visitToPatch, visitFromRepository);

                await _visitsRepository.UpdateVisitAsync(updatedVisit);

                return Ok("Visit has been updated.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in PartialVisitUpdateAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }




        [HttpDelete("DeleteVisit/{visitId}")]
        public async Task<IActionResult> DeleteVisitAsync(Guid visitId)
        {
            try
            {
                var visitForDelete = _visitsRepository.ReadVisitByVisitIdAsync(visitId).Result;

                if (visitForDelete == null)
                    return BadRequest("A visit with this id can not be found");

                await _visitsRepository.DeleteVisitAsync(visitForDelete);

                return Ok("Visit has been deleted.");
            }
            catch (Exception e)
            {
                Log.Logger.Error("This exception caught in DeleteVisitAsync endpoint.");
                Log.Logger.Error("Error message: " + e.Message);
                throw;
            }
        }
    }
}
