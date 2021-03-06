// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SF.SGL.API.Infra.Contexto;

#nullable disable

namespace SF.SGL.API.Migrations
{
    [DbContext(typeof(SGLContexto))]
    [Migration("20211215014134_Inicial")]
    partial class Inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SF.SGL.API.Dominio.Entidades.EntidadeMonitoramento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cd_monitoramento ");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Acao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("nm_acao_contigencia");

                    b.Property<string>("Contato")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nm_contato_responsavel");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("nm_monitoramento_descricao");

                    b.Property<int?>("EntidadeSistemaId")
                        .IsRequired()
                        .HasColumnType("int")
                        .HasColumnName("cd_sistema");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("gd_monitoramento");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("nm_monitoramento ");

                    b.HasKey("Id");

                    b.HasIndex("EntidadeSistemaId");

                    b.ToTable("monitoramento", "dbo");
                });

            modelBuilder.Entity("SF.SGL.API.Dominio.Entidades.EntidadeParametroExpurgo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cd_parametro_expurgo_log");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EntidadeSistemaId")
                        .HasColumnType("int")
                        .HasColumnName("cd_sistema");

                    b.Property<int>("ParametroExpurgoLogAuditoria")
                        .HasMaxLength(3)
                        .HasColumnType("int")
                        .HasColumnName("pr_expurgo_log_auditoria");

                    b.Property<int>("ParametroExpurgoLogOperacao")
                        .HasMaxLength(3)
                        .HasColumnType("int")
                        .HasColumnName("pr_expurgo_log_operacao");

                    b.HasKey("Id");

                    b.HasIndex("EntidadeSistemaId");

                    b.ToTable("parametro_expurgo_log", "dbo");
                });

            modelBuilder.Entity("SF.SGL.API.Dominio.Entidades.EntidadeSistema", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cd_sistema");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("nm_sistema");

                    b.Property<string>("UrlServicoConsultaLog")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("nm_url_servico_consulta_log");

                    b.Property<string>("UsuarioLogin")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("nm_usuario_servico_consulta_log");

                    b.Property<string>("UsuarioSenha")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("nm_senha_servico_consulta_log");

                    b.HasKey("Id");

                    b.ToTable("sistema", "dbo");
                });

            modelBuilder.Entity("SF.SGL.API.Dominio.Entidades.EntidadeMonitoramento", b =>
                {
                    b.HasOne("SF.SGL.API.Dominio.Entidades.EntidadeSistema", "EntidadeSistema")
                        .WithMany()
                        .HasForeignKey("EntidadeSistemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EntidadeSistema");
                });

            modelBuilder.Entity("SF.SGL.API.Dominio.Entidades.EntidadeParametroExpurgo", b =>
                {
                    b.HasOne("SF.SGL.API.Dominio.Entidades.EntidadeSistema", "EntidadeSistema")
                        .WithMany()
                        .HasForeignKey("EntidadeSistemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EntidadeSistema");
                });
#pragma warning restore 612, 618
        }
    }
}
