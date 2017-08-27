﻿using System;
using NeptunLight.DataAccess;
using Xunit;

namespace NeptunLight.Tests
{
    public class WebNeptunInterfaceTests
    {
        private static INeptunInterface CreateInterface()
        {
            WebNeptunInterfaceFactory factory = new WebNeptunInterfaceFactory
            {
                BaseUri = new Uri("https://neptun3r.web.uni-corvinus.hu/hallgatoi_2/"),
                Username = Environment.GetEnvironmentVariable("NEPTUN_USERNAME", EnvironmentVariableTarget.User),
                Password = Environment.GetEnvironmentVariable("NEPTUN_PASSWORD", EnvironmentVariableTarget.User)
            };
            INeptunInterface client = factory.Build();
            return client;
        }

        [Fact]
        public async void Login_WrongCredentials()
        {
            WebNeptunInterfaceFactory factory = new WebNeptunInterfaceFactory
            {
                BaseUri = new Uri("https://neptun3r.web.uni-corvinus.hu/hallgatoi_2/"),
                Username = "random",
                Password = "asd"
            };
            INeptunInterface client = factory.Build();
            await Assert.ThrowsAnyAsync<UnauthorizedAccessException>(() => client.LoginAsync());
        }

        [Fact]
        public async void Login_CorrectCredentials()
        {
            INeptunInterface client = CreateInterface();
            await client.LoginAsync();
        }

        [Fact]
        public async void Messages_MoreThan200()
        {
            INeptunInterface client = CreateInterface();
            var messages = await client.RefreshMessagesAsnyc();
            Assert.InRange(messages.Count, 201, Int32.MaxValue);
        }
    }
}