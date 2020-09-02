using Autofac.Extras.Moq;
using FIL.Api.QueryHandlers.Users;
using FIL.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;

namespace FIL.Api.Tests.UnitTests.QueryHandlers.User
{
    [TestFixture]
    public class LoginUserQueryHandlerTests : BaseTestFixture
    {
        private AutoMock GetMock(PasswordVerificationResult desiredResult)
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IPasswordHasher<string>>()
                .Setup(h => h.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(desiredResult);
            return mock;
        }

        [Test]
        public void CorrectPassword_ReturnsSuccess()
        {
            using (var mock = GetMock(PasswordVerificationResult.Success))
            {
                mock.Mock<IUserRepository>().Setup(r => r.GetByEmail(It.IsAny<string>()))
                    .Returns(new Contracts.DataModels.User
                    {
                        Password = "test",
                        LockOutEnabled = false
                    });

                var queryHander = mock.Create<LoginUserQueryHandler>();
                var result = queryHander.Handle(new Contracts.Queries.User.LoginUserQuery
                {
                    Password = "test"
                });

                result.Success.ShouldBeTrue();
            }
        }

        [Test]
        public void WrongPassword_IncrementsLockOut()
        {
            using (var mock = GetMock(PasswordVerificationResult.Failed))
            {
                var userRepo = mock.Mock<IUserRepository>();
                userRepo.Setup(r => r.GetByEmail(It.IsAny<string>()))
                    .Returns(new Contracts.DataModels.User
                    {
                        Password = "test",
                        LockOutEnabled = false
                    });

                var queryHander = mock.Create<LoginUserQueryHandler>();
                var result = queryHander.Handle(new Contracts.Queries.User.LoginUserQuery
                {
                    Password = "test1"
                });

                result.Success.ShouldBeFalse();
                userRepo.Verify(c => c.Save(It.Is<Contracts.DataModels.User>(u => u.AccessFailedCount == 1)));
            }
        }

        [Test]
        public void FifthFailureLockout_SetsLockOut()
        {
            using (var mock = GetMock(PasswordVerificationResult.Failed))
            {
                var userRepo = mock.Mock<IUserRepository>();
                userRepo.Setup(r => r.GetByEmail(It.IsAny<string>()))
                    .Returns(new Contracts.DataModels.User
                    {
                        Password = "test",
                        LockOutEnabled = false,
                        AccessFailedCount = 4
                    });

                var queryHander = mock.Create<LoginUserQueryHandler>();
                var result = queryHander.Handle(new Contracts.Queries.User.LoginUserQuery
                {
                    Password = "test1"
                });

                result.Success.ShouldBeFalse();
                userRepo.Verify(c => c.Save(It.Is<Contracts.DataModels.User>(u => u.LockOutEnabled == true)));
            }
        }

        [Test]
        public void CorrectPassword_IgnoredDuringLockOut()
        {
            using (var mock = GetMock(PasswordVerificationResult.Success))
            {
                var userRepo = mock.Mock<IUserRepository>();
                userRepo.Setup(r => r.GetByEmail(It.IsAny<string>()))
                    .Returns(new Contracts.DataModels.User
                    {
                        Password = "test",
                        LockOutEnabled = true,
                        LockOutEndDateUtc = DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                        AccessFailedCount = 5
                    });

                var queryHander = mock.Create<LoginUserQueryHandler>();
                var result = queryHander.Handle(new Contracts.Queries.User.LoginUserQuery
                {
                    Password = "test"
                });

                result.Success.ShouldBeFalse();
                userRepo.Verify(c => c.Save(It.Is<Contracts.DataModels.User>(u => u.LockOutEnabled == true)));
            }
        }

        [Test]
        public void CorrectPassword_ResetsLockOutAfterLockOut()
        {
            using (var mock = GetMock(PasswordVerificationResult.Success))
            {
                var userRepo = mock.Mock<IUserRepository>();
                userRepo.Setup(r => r.GetByEmail(It.IsAny<string>()))
                    .Returns(new Contracts.DataModels.User
                    {
                        Password = "test",
                        LockOutEnabled = true,
                        LockOutEndDateUtc = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)),
                        AccessFailedCount = 5
                    });

                var queryHander = mock.Create<LoginUserQueryHandler>();
                var result = queryHander.Handle(new Contracts.Queries.User.LoginUserQuery
                {
                    Password = "test"
                });

                result.Success.ShouldBeTrue();
                userRepo.Verify(c => c.Save(It.Is<Contracts.DataModels.User>(u => u.LockOutEnabled == false && u.AccessFailedCount == 0)));
            }
        }

        [Test]
        public void CorrectPassword_ResetsAccessFailedCount()
        {
            using (var mock = GetMock(PasswordVerificationResult.Success))
            {
                var userRepo = mock.Mock<IUserRepository>();
                userRepo.Setup(r => r.GetByEmail(It.IsAny<string>()))
                    .Returns(new Contracts.DataModels.User
                    {
                        Password = "test",
                        LockOutEnabled = false,
                        AccessFailedCount = 4
                    });

                var queryHander = mock.Create<LoginUserQueryHandler>();
                var result = queryHander.Handle(new Contracts.Queries.User.LoginUserQuery
                {
                    Password = "test"
                });

                result.Success.ShouldBeTrue();
                userRepo.Verify(c => c.Save(It.Is<Contracts.DataModels.User>(u => u.AccessFailedCount == 0)));
            }
        }
    }
}