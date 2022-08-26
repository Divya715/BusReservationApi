﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using BusReservationApi.Models;
using Microsoft.EntityFrameworkCore;
using BusReservationApi.ViewModel;

namespace BusReservationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : ControllerBase
    {
        BusReservationContext db = new BusReservationContext();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = db.BusInfo.FromSqlInterpolated<BusInfo>($"BusList");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("{BusId}")]
        public IActionResult GetBus(int BusId)
        {
            try
            {
                var data = db.buses.Where(B => B.BusId == BusId).FirstOrDefault();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("seatsavb/{BusId}")]
        public IActionResult GetBusAvbSeats(int BusId)
        {
            try
            {
                var data = db.BusSeats.Where(busSeat => busSeat.BusId == BusId && busSeat.Available.Equals(true)).Count();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }


        [HttpPut]
        [Route("editbus/{BusId}")]
        public IActionResult PutBus(int BusId, Bus bus)
        {
            var data = db.buses.Where(buses => buses.BusId == BusId).FirstOrDefault();
            try
            {
                if (ModelState.IsValid)
                {
                    data.BusNo = bus.BusNo;
                    data.Rows = bus.Rows;
                    data.Dtime = bus.Dtime;
                    data.Atime = bus.Atime;
                    data.Pickup = bus.Pickup;
                    data.SeatCost = bus.SeatCost;
                    data.DriverName = bus.DriverName;
                    data.DriverContact = bus.DriverContact;
                    data.TypeOfBus = bus.TypeOfBus;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("addbus")]
        public IActionResult PostBus(Bus bus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.buses.Add(bus);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            db.SaveChanges();
            return Created("Record Successfully Added", bus);
        }

        [HttpDelete]
        [Route("deletebus/{BusId}")]
        public IActionResult DeleteBus(int BusId)
        {
            try
            {
                var data = db.buses.Where(buses => buses.BusId == BusId).FirstOrDefault();
                db.buses.Remove(data);
                db.SaveChanges();
                return Ok("Successfully deleted the bus");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }

        }
    }
}

