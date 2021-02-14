using System.Linq;
using System.Net;
using dotnetpostgres.Api.ViewModels;
using dotnetpostgres.Api.ViewModels.Parameter.Request;
using dotnetpostgres.Api.ViewModels.Parameter.Response;
using dotnetpostgres.Dto.Parameter;
using dotnetpostgres.Request;
using dotnetpostgres.Request.Criteria.Parameter;
using dotnetpostgres.Response;
using dotnetpostgres.Services.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetpostgres.Api.Controllers
{
    [Route("parameters"), ApiController, Authorize]
    public class ParameterController : ApiControllerBase
    {
        private readonly IParameterService _parameterService;

        public ParameterController(IParameterService parameterService)
        {
            _parameterService = parameterService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedListResponse<ParameterViewModel>>), (int)HttpStatusCode.OK)]
        public IActionResult Get([FromQuery] SearchParameterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = Search(model);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<int>), (int)HttpStatusCode.OK)]
        public IActionResult Post([FromBody] CreateParameterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = Create(model);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public IActionResult Put([FromRoute] IdViewModel idModel, [FromBody] UpdateParameterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = Update(idModel.Id, model);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public IActionResult Delete([FromRoute] IdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = Delete(model.Id);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        private ApiResponse<PagedListResponse<ParameterViewModel>> Search(SearchParameterViewModel model)
        {
            var apiResp = new ApiResponse<PagedListResponse<ParameterViewModel>>
            {
                Type = ResponseType.Fail,
                Data = new PagedListResponse<ParameterViewModel>()
            };

            var request = new FilteredPagedListRequest<SearchParameterCriteria>
            {
                FilterCriteria = new SearchParameterCriteria
                {
                    Name = model.Name
                },
                IncludeRecordsTotal = model.IncludeRecordsTotal,
                Limit = model.Limit,
                Offset = model.Offset
            };

            var resp = _parameterService.Search(request);

            apiResp.Data.Items = resp.Items.Select(p => new ParameterViewModel
            {
                Id = p.Id,
                TypeId = p.TypeId,
                Order = p.Order,
                Value = p.Value
            });

            apiResp.Data.RecordsTotal = resp.RecordsTotal;
            apiResp.Type = ResponseType.Success;

            return apiResp;
        }

        private ApiResponse Create(CreateParameterViewModel model)
        {
            var apiResp = new ApiResponse
            {
                Type = ResponseType.Fail
            };
            var parameter = new Parameter
            {
                Value = model.Value,
                Order = model.Order.Value,
                TypeId = model.TypeId.Value
            };

            var resp = _parameterService.Add(parameter);

            if (resp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = resp.ErrorCode;
                return apiResp;
            }

            apiResp.Type = ResponseType.Success;
            return apiResp;
        }
        private ApiResponse Update(int id, UpdateParameterViewModel model)
        {
            var apiResp = new ApiResponse
            {
                Type = ResponseType.Fail
            };

            var parameter = new Parameter
            {
                Id = id,
                Value = model.Value,
                Order = model.Order.Value,
                TypeId = model.TypeId.Value
            };

            var resp = _parameterService.Edit(parameter);

            if (resp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = resp.ErrorCode;
                return apiResp;
            }

            apiResp.Type = ResponseType.Success;
            return apiResp;
        }

        private ApiResponse Delete(int parameterId)
        {
            var apiResp = new ApiResponse
            {
                Type = ResponseType.Fail
            };

            var resp = _parameterService.SoftDelete(parameterId);

            if (resp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = resp.ErrorCode;
                return apiResp;
            }

            apiResp.Type = ResponseType.Success;
            return apiResp;
        }
    }
}