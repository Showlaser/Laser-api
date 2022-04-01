﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LaserAPI.Models.Helper
{
    public class ControllerErrorHandler
    {
        public async Task<ActionResult<T>> Execute<T>(Task<T> task)
        {
            try
            {
                return await task.WaitAsync(CancellationToken.None);
            }
            catch (InvalidDataException)
            {
                return new BadRequestResult();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }
            catch (DuplicateNameException)
            {
                return new ConflictResult();
            }
            catch (SecurityException)
            {
                return new UnauthorizedResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> Execute(Task task)
        {
            try
            {
                await task.WaitAsync(CancellationToken.None);
                return new OkResult();
            }
            catch (InvalidDataException)
            {
                return new BadRequestResult();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }
            catch (DuplicateNameException)
            {
                return new ConflictResult();
            }
            catch (SecurityException)
            {
                return new UnauthorizedResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public ActionResult Execute(Action action)
        {
            try
            {
                action.Invoke();
                return new OkResult();
            }
            catch (InvalidDataException)
            {
                return new BadRequestResult();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }
            catch (DuplicateNameException)
            {
                return new ConflictResult();
            }
            catch (SecurityException)
            {
                return new UnauthorizedResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public ActionResult<T> Execute<T>(Func<T> action)
        {
            try
            {
                return action.Invoke();
            }
            catch (InvalidDataException)
            {
                return new BadRequestResult();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }
            catch (DuplicateNameException)
            {
                return new ConflictResult();
            }
            catch (SecurityException)
            {
                return new UnauthorizedResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}