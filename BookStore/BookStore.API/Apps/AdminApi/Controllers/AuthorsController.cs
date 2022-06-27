using AutoMapper;
using BookStore.Api.DTOs.AuthorDTOs;
using BookStore.Core.Entities;
using BookStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Api.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AuthorsController(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult Create(AuthorPostDTO authorPostDTO)
        {
            Author author = new Author
            {
                BornYear = authorPostDTO.BornYear,
                FullName = authorPostDTO.FullName
            };
            _context.Authors.Add(author);
            _context.SaveChanges();
            AuthorGetDTO authorGet = _mapper.Map<AuthorGetDTO>(author);
            return StatusCode(201, new { authorGet });
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (author == null)
            {
                return NotFound();
            }
            AuthorGetDTO authorGetDTO =_mapper.Map<AuthorGetDTO>(author);
            return Ok(authorGetDTO);
        }
        [Authorize(Roles ="SuperAdmin,Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<AuthorListItemDTO> authorListItems = new List<AuthorListItemDTO>();
            authorListItems = _mapper.Map<List<AuthorListItemDTO>>(_context.Authors.Include(x => x.Books).Where(x => x.IsDeleted == false).ToList());
            return Ok(authorListItems);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if(author==null)
            {
                return BadRequest();
            }
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id,AuthorPostDTO authorPost)
        {
            Author author=_context.Authors.FirstOrDefault(x => x.Id == id &&!x.IsDeleted);
            if (author == null)
            {
                return BadRequest();
            }
            author.FullName=authorPost.FullName;
            author.BornYear = authorPost.BornYear;
            author.ModifiedAt = System.DateTime.UtcNow.AddHours(4);
            _context.SaveChanges();
            return Ok();
        }
    }
}
