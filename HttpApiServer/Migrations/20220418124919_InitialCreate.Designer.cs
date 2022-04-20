﻿// <auto-generated />
using System;
using HttpApiServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HttpApiServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220418124919_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("HttpModels.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("HttpModels.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("HttpModels.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CartId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("HttpModels.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("55e73c65-79ce-4cc6-876b-0b4851937754"),
                            Name = "Смарфоны"
                        },
                        new
                        {
                            Id = new Guid("1a9a7dfd-1044-4cab-93a1-85ec1d4de06f"),
                            Name = "USB накопители"
                        },
                        new
                        {
                            Id = new Guid("ca2b9538-e3df-460e-b931-dcc4c70ae991"),
                            Name = "Ноутбуки"
                        },
                        new
                        {
                            Id = new Guid("5b9a73dd-72cc-4aa3-baf4-a5187f79d414"),
                            Name = "Наушники"
                        },
                        new
                        {
                            Id = new Guid("f49f9907-42ec-405b-bb14-0745723dc36b"),
                            Name = "Игровые консоли"
                        },
                        new
                        {
                            Id = new Guid("63541433-526f-493c-ada6-b89ae74eb657"),
                            Name = "Телевизоры"
                        });
                });

            modelBuilder.Entity("HttpModels.ConfirmationCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ConfirmationCodes");
                });

            modelBuilder.Entity("HttpModels.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HttpModels.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("HttpModels.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UrlImage")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0ae78134-55e9-4429-b888-c3410ffb0382"),
                            CategoryId = new Guid("55e73c65-79ce-4cc6-876b-0b4851937754"),
                            Name = "Смартфон Google Pixel 5a",
                            Price = 560m,
                            Quantity = 20,
                            UrlImage = "https://cdn1.ozone.ru/s3/multimedia-3/wc250/6237328203.jpg"
                        },
                        new
                        {
                            Id = new Guid("cdb94779-8030-4d4c-afa7-ed12948dcbed"),
                            CategoryId = new Guid("1a9a7dfd-1044-4cab-93a1-85ec1d4de06f"),
                            Name = "USB накопитель Samsung",
                            Price = 30m,
                            Quantity = 35,
                            UrlImage = "https://cdn1.ozone.ru/multimedia/wc1200/1026248251.jpg"
                        },
                        new
                        {
                            Id = new Guid("7422f001-499c-442f-b472-38709727aa32"),
                            CategoryId = new Guid("ca2b9538-e3df-460e-b931-dcc4c70ae991"),
                            Name = "Ноутбук Lenovo IdeaPad 4 15IX5P6",
                            Price = 830.50m,
                            Quantity = 15,
                            UrlImage = "https://cdn1.ozone.ru/s3/multimedia-7/wc1200/6166994971.jpg"
                        },
                        new
                        {
                            Id = new Guid("4d39555e-8997-477e-be78-5e6168a61b91"),
                            CategoryId = new Guid("5b9a73dd-72cc-4aa3-baf4-a5187f79d414"),
                            Name = "Наушники Sony WH-CH56030NW",
                            Price = 130.60m,
                            Quantity = 25,
                            UrlImage = "https://cdn1.ozone.ru/s3/multimedia-r/wc1200/6179635779.jpg"
                        },
                        new
                        {
                            Id = new Guid("d9582f94-beb5-4cfc-bf6c-5f699bf75497"),
                            CategoryId = new Guid("f49f9907-42ec-405b-bb14-0745723dc36b"),
                            Name = "Microsoft Xbox Series X",
                            Price = 985m,
                            Quantity = 2,
                            UrlImage = "https://cdn1.ozone.ru/s3/multimedia-l/wc1200/6232471137.jpg"
                        },
                        new
                        {
                            Id = new Guid("8794987d-f4ad-4335-9726-4240327b1d0d"),
                            CategoryId = new Guid("63541433-526f-493c-ada6-b89ae74eb657"),
                            Name = "Телевизор Sony KD50X81J 50",
                            Price = 1000.89m,
                            Quantity = 15,
                            UrlImage = "https://cdn1.ozone.ru/s3/multimedia-n/wc1200/6068732087.jpg"
                        });
                });

            modelBuilder.Entity("HttpModels.CartItem", b =>
                {
                    b.HasOne("HttpModels.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HttpModels.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("HttpModels.OrderItem", b =>
                {
                    b.HasOne("HttpModels.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HttpModels.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("HttpModels.Product", b =>
                {
                    b.HasOne("HttpModels.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("HttpModels.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("HttpModels.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("HttpModels.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
