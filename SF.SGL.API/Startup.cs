namespace SF.SGL.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SF.SGL.API", Version = "v1" });
            c.CustomSchemaIds(type => type.ToString());
        });

        services.AddMediatR(typeof(Startup).Assembly);

        services.AddAutoMapper(typeof(Startup).Assembly);

        services.AddDbContext<SGLContexto>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DetaultConnection"),
            b => b.MigrationsAssembly(typeof(SGLContexto).Assembly.FullName)));

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SGLContexto contexto)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SF.SGL.API v1"));
        }
        app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader()
                   .SetIsOriginAllowed(origin => true)
                   .AllowCredentials());

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        contexto.Database.Migrate();
    }
}
