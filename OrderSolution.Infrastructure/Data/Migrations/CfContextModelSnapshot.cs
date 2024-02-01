﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OrderSolution.Infrastructure.Data;

#nullable disable

namespace OrderSolution.Infrastructure.Data.Migrations
{
    [DbContext(typeof(CfContext))]
    partial class CfContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.CustomerAddressEntity", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("CustomerId");

                    b.ToTable("CustomerAddresses");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.CustomerDetailEntity", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("CustomerId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("CustomerDetails");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.CustomerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.OrderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PaidAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("Numeric(7,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.OrderItemEntity", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("ArticleNumber")
                        .HasColumnType("text");

                    b.Property<Guid?>("OrderEntityId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("Numeric(7,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("Numeric(7,2)");

                    b.HasKey("OrderId", "ArticleNumber");

                    b.HasIndex("OrderEntityId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.CustomerAddressEntity", b =>
                {
                    b.HasOne("OrderSolution.Infrastructure.Entities.CustomerEntity", "Customer")
                        .WithOne("CustomerAddress")
                        .HasForeignKey("OrderSolution.Infrastructure.Entities.CustomerAddressEntity", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.CustomerDetailEntity", b =>
                {
                    b.HasOne("OrderSolution.Infrastructure.Entities.CustomerEntity", "Customer")
                        .WithOne("CustomerDetail")
                        .HasForeignKey("OrderSolution.Infrastructure.Entities.CustomerDetailEntity", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.OrderEntity", b =>
                {
                    b.HasOne("OrderSolution.Infrastructure.Entities.CustomerEntity", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.OrderItemEntity", b =>
                {
                    b.HasOne("OrderSolution.Infrastructure.Entities.OrderEntity", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderEntityId");
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.CustomerEntity", b =>
                {
                    b.Navigation("CustomerAddress")
                        .IsRequired();

                    b.Navigation("CustomerDetail")
                        .IsRequired();
                });

            modelBuilder.Entity("OrderSolution.Infrastructure.Entities.OrderEntity", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
