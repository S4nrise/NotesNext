namespace NotesApiNext.Middlewares
{
    public  static class ExceptionMiddlewareExtencion
    {
        public static IApplicationBuilder UseException(this IApplicationBuilder builder) 
            => builder.UseMiddleware<ExceptionMiddleware>();
    }
}
