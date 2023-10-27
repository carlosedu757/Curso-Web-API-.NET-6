using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork context, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _uof = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var categoria = _uof.CategoriaRepository.GetCategoriasProdutos();
            var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);
            return categoriaDTO;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriaDTO;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um problema ao tratar a sua solicitação. {ex}");
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);
                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                if (categoria == null)
                    return NotFound($"Categoria com id = {id} não encontrada...");

                return Ok(categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoriaDTO categoriaDTO)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDTO);

                if (categoria is null)
                    return BadRequest();

                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDto.CategoriaId }, categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDTO)
        {
            try
            {
                if (id != categoriaDTO.CategoriaId)
                    return BadRequest();

                var categoria = _mapper.Map<Categoria>(categoriaDTO);

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            { 
                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria is null)
                    return NotFound($"Categoria com id = {id} não encontrada...");

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }
    }
}
