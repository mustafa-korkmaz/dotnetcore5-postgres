﻿using dotnetpostgres.Dto;
using System;
using System.Collections.Generic;
using dotnetpostgres.Response;

namespace dotnetpostgres.Services
{
    public interface ICrudService<TDto>
        where TDto : DtoBase
    {
        /// <summary>
        /// Indicates the owner of an entity. i.e ApplicationUser.Id
        /// when we need to validate entity owner we will need this owner UserId property
        /// </summary>
        Guid OwnerId { get; set; }

        /// <summary>
        /// creates new entity from given dto
        /// </summary>
        /// <param name="dto"></param>
        Response.Response Add(TDto dto);

        /// <summary>
        /// creates new entities as bulk insert from given dto list
        /// </summary>
        /// <param name="dtoList"></param>
        Response.Response AddRange(TDto[] dtoList);

        /// <summary>
        /// updates given entity and returns affected row count.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>affected row count in db</returns>
        DataResponse<int> Edit(TDto dto);

        /// <summary>
        /// hard deletes entity by given id
        /// </summary>
        /// <param name="id"></param>
        Response.Response Delete(object id);

        /// <summary>
        /// soft deletes entity
        /// </summary>
        /// <param name="id"></param>
        Response.Response SoftDelete(object id);

        /// <summary>
        /// returns dto object by given id
        /// </summary>
        /// <param name="id"></param>
        DataResponse<TDto> Get(object id);

        /// <summary>
        /// returns all dto objects
        /// </summary>
        DataResponse<IEnumerable<TDto>> GetAll();
    }
}