using FluentAssertions;
using ISF.FAF_RF.Domain.Common;
using LibraryPepper.API.Books.Models;
using LibraryPepper.Application.Books.Commands.DeleteBook;
using LibraryPepper.Domain.Entities;
using LibraryPepper.Domain.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryPepper.Tests.UnitTest.Application.Books.Commands.DeleteBook
{
    public class PublishBookCommandTest
    {
        private readonly Mock<IBookRepository> _bookRepository;
        private readonly DeleteBookCommandHandler _commandHandler;
        private readonly CancellationToken _cancellationToken;

        public PublishBookCommandTest()
        {
            _bookRepository = new Mock<IBookRepository>();
            _commandHandler = new DeleteBookCommandHandler(_bookRepository.Object);
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async void ShouldDeleteBook()
        {
            var command = new DeleteBookCommand(Id: 1);
            var book = new Book { Id = command.Id };
            _bookRepository
                .Setup(x => x.GetById(command.Id, _cancellationToken))
                .ReturnsAsync(book);
            _bookRepository
                .Setup(x => x.Delete(book, _cancellationToken))
                .ReturnsAsync(true);

            var result = await _commandHandler.Handle(command, _cancellationToken);

            result.Should().Be(true);
            _bookRepository
                .Verify(x => x.GetById(command.Id, _cancellationToken), Times.Once);
            _bookRepository
                .Verify(x => x.Delete(book, _cancellationToken), Times.Once);
        }
    }
}
