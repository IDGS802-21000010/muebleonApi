using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using muebleon.DTOs;
using muebleon.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        private readonly MuebleonContext _baseDatos;
        //, IConfiguration configuration
        public LoginController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
            //_configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {

            var usuario = _baseDatos.Usuarios.SingleOrDefault(u => u.Usuario1 == loginDto.Usuario && u.Contrasenia == loginDto.Contrasenia);
            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            if (usuario.Rol == "admin")
            {
                var empleado = _baseDatos.Empleados.SingleOrDefault(e => e.IdUsuario == usuario.IdUsuario);
                if (empleado == null)
                {
                    return Unauthorized("No se encontró el empleado asociado.");
                }

                //var token = GenerateToken(usuario.Usuario1, usuario.Rol);
                /*return Ok(new AuthResponseDto
                {
                    Token = token,
                    IsSuccess = true,
                    Message = "Login Success."
                });*/
                return Ok("Empleado");
            }
            else if (usuario.Rol == "cliente")
            {
                var cliente = _baseDatos.Clientes.SingleOrDefault(c => c.IdUsuario == usuario.IdUsuario);
                if (cliente == null)
                {
                    return Unauthorized("No se encontro el cliente.");
                }
                //var token = GenerateToken(usuario.Usuario1, usuario.Rol);
                /*return Ok(new AuthResponseDto
                {
                    Token = token,
                    IsSuccess = true,
                    Message = "Login Success."
                });*/
                return Ok("Cliente");
            }

            return BadRequest("Rol de usuario no reconocido.");
        }

        /*public string GenerateToken(string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWTSetting:securityKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }*/
    }
}
