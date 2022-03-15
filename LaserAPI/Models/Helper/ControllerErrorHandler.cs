using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LaserAPI.Models.Helper
{
    public class ControllerErrorHandler
    {
        public int StatusCode { get; private set; }

        public async Task<T> Execute<T>(Task<T> task)
        {
            T result = default;
            try
            {
                result = await task.WaitAsync(CancellationToken.None);
                StatusCode = StatusCodes.Status200OK;
            }
            catch (InvalidDataException)
            {
                StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (KeyNotFoundException)
            {
                StatusCode = StatusCodes.Status404NotFound;
            }
            catch (DbUpdateException)
            {
                StatusCode = StatusCodes.Status304NotModified;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                StatusCode = StatusCodes.Status500InternalServerError;
            }

            return result;
        }

        public async Task Execute(Task task)
        {
            try
            {
                await task.WaitAsync(CancellationToken.None);
                StatusCode = StatusCodes.Status200OK;
            }
            catch (InvalidDataException)
            {
                StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (KeyNotFoundException)
            {
                StatusCode = StatusCodes.Status404NotFound;
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine(exception);
                StatusCode = StatusCodes.Status304NotModified;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }

        public T Execute<T>(Func<T> task)
        {
            T result = default;
            try
            {
                result = task.Invoke();
                StatusCode = StatusCodes.Status200OK;
            }
            catch (InvalidDataException)
            {
                StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (KeyNotFoundException)
            {
                StatusCode = StatusCodes.Status404NotFound;
            }
            catch (DbUpdateException)
            {
                StatusCode = StatusCodes.Status304NotModified;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                StatusCode = StatusCodes.Status500InternalServerError;
            }

            return result;
        }

        public void Execute(Action task)
        {
            try
            {
                task.Invoke();
                StatusCode = StatusCodes.Status200OK;
            }
            catch (InvalidDataException)
            {
                StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (KeyNotFoundException)
            {
                StatusCode = StatusCodes.Status404NotFound;
            }
            catch (DbUpdateException)
            {
                StatusCode = StatusCodes.Status304NotModified;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}