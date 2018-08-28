using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Data;
using User.API.Models;
using User.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    public class AppUserController : Controller
    {
        private readonly IRepository<AppUser, int> _repository;

        public AppUserController(IRepository<AppUser, int> repository)
        {
            _repository = repository;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _repository.FindAsync();
            return new JsonResult(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repository.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AppUser appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _repository.FindAsync(a => a.Name == appUser.Name || a.Phone == appUser.Phone);
            if (user.Count() == 1)
            {
                throw new UserFriendlyException("user or tel is already exist");
            }
            int id = await _repository.CreateAsync(appUser);
            return Ok(id);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]AppUser appUser)
        {
            var user = await _repository.FindAsync(a => a.Id != appUser.Id && (a.Name == appUser.Name || a.Phone == appUser.Phone));
            if (user.Count() == 1)
            {
                throw new UserFriendlyException("user or tel is already exist");
            }
            await _repository.UpdateAsync(appUser);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task Delete(int id)
        {
            await _repository.DeleteAsync(id);
        }

        [Route("check-or-create")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreate(string phone)
        {
           // throw new  HttpRequestException("error");
            var user = await _repository.SingleAsync(a => a.Phone == phone);
            if (user==null)
            {
                user = new AppUser {
                    Phone=phone
                };
                await _repository.CreateAsync(user);
            }
            return Ok(user.Id);
        }
    }
}
