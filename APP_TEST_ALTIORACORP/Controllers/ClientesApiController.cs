using APP_TEST_ALTIORACORP.Data;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace APP_TEST_ALTIORACORP.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ClientesApiController : Controller
    {
        private AltioraContext contexto;

        public ClientesApiController(AltioraContext context) {
            this.contexto = context;
        }

        [HttpGet]
        public IActionResult Seleccionar(DataSourceLoadOptions loadOptions) {
            var clientes = contexto.Clientes.Select(i => new {
                i.IDENTIFICACION,
                i.NOMBRES,
                i.APELLIDOS,
                i.DIRECCION,
                i.TELEFONO
            });
            return Json(DataSourceLoader.Load(clientes, loadOptions));
        }

        [HttpPost]
        public IActionResult Insertar(string values) {
            var model = new Clientes();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            //if (ValidacionCliente(nameof(Clientes.IDENTIFICACION)))
                //return View("Index");

            var result = contexto.Clientes.Add(model);
            contexto.SaveChanges();

            return Json(result.Entity.IDENTIFICACION);
                       
        }

        [HttpPut]
        public IActionResult Actualizar(string key, string values) {
            var model = contexto.Clientes.FirstOrDefault(item => item.IDENTIFICACION == key);
            if(model == null)
                return StatusCode(409, "Clientes not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            contexto.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Eliminar(string key) {
            var model = contexto.Clientes.FirstOrDefault(item => item.IDENTIFICACION == key);

            contexto.Clientes.Remove(model);
            contexto.SaveChanges();
        }

        protected string GetErrorMessage()
        {
            return $"Ya existe una identificación igual a la ingresada";
        }

        private void PopulateModel(Clientes model, IDictionary values) {
            string IDENTIFICACION = nameof(Clientes.IDENTIFICACION);
            string NOMBRES = nameof(Clientes.NOMBRES);
            string APELLIDOS = nameof(Clientes.APELLIDOS);
            string DIRECCION = nameof(Clientes.DIRECCION);
            string TELEFONO = nameof(Clientes.TELEFONO);

            if(values.Contains(IDENTIFICACION)) {
                model.IDENTIFICACION = Convert.ToString(values[IDENTIFICACION]);
            }

            if(values.Contains(NOMBRES)) {
                model.NOMBRES = Convert.ToString(values[NOMBRES]);
            }

            if(values.Contains(APELLIDOS)) {
                model.APELLIDOS = Convert.ToString(values[APELLIDOS]);
            }

            if(values.Contains(DIRECCION)) {
                model.DIRECCION = Convert.ToString(values[DIRECCION]);
            }

            if(values.Contains(TELEFONO)) {
                model.TELEFONO = Convert.ToString(values[TELEFONO]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

        private string ClienteExistente(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join("Actualmente ya se encuentra un registro con la identificación registrada ", messages);
        }

        private bool ValidacionCliente(string id) {

            var model = contexto.Clientes.FirstOrDefault(item => item.IDENTIFICACION == id);
            return true;
        }

        

    }
}