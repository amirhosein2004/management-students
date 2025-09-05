using AutoMapper;
using Moq;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models;
using StudentManagementAPI.Repositories;
using StudentManagementAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagementAPI.Tests
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            _mockRepo = new Mock<IStudentRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new StudentService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName = "John", LastName = "Doe" },
                new Student { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };

            var studentDtos = new List<StudentDTO>
            {
                new StudentDTO { Id = 1, FirstName = "John", LastName = "Doe" },
                new StudentDTO { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };

            _mockRepo.Setup(repo => repo.GetAllStudentsAsync()).ReturnsAsync(students);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<StudentDTO>>(students)).Returns(studentDtos);

            // Act
            var result = await _service.GetAllStudentsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("John", result.First().FirstName);
            Assert.Equal("Smith", result.Last().LastName);
        }

        [Fact]
        public async Task GetStudentByIdAsync_WithValidId_ShouldReturnStudent()
        {
            // Arrange
            var studentId = 1;
            var student = new Student { Id = studentId, FirstName = "John", LastName = "Doe" };
            var studentDto = new StudentDTO { Id = studentId, FirstName = "John", LastName = "Doe" };

            _mockRepo.Setup(repo => repo.GetStudentByIdAsync(studentId)).ReturnsAsync(student);
            _mockMapper.Setup(mapper => mapper.Map<StudentDTO>(student)).Returns(studentDto);

            // Act
            var result = await _service.GetStudentByIdAsync(studentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(studentId, result.Id);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetStudentByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var studentId = 999;
            _mockRepo.Setup(repo => repo.GetStudentByIdAsync(studentId)).ReturnsAsync((Student)null);

            // Act
            var result = await _service.GetStudentByIdAsync(studentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateStudentAsync_ShouldCreateAndReturnStudent()
        {
            // Arrange
            var createDto = new CreateStudentDTO
            {
                FirstName = "New",
                LastName = "Student",
                Email = "new.student@example.com"
            };

            var student = new Student
            {
                Id = 3,
                FirstName = "New",
                LastName = "Student",
                Email = "new.student@example.com"
            };

            var studentDto = new StudentDTO
            {
                Id = 3,
                FirstName = "New",
                LastName = "Student",
                Email = "new.student@example.com"
            };

            _mockMapper.Setup(mapper => mapper.Map<Student>(createDto)).Returns(student);
            _mockRepo.Setup(repo => repo.CreateStudentAsync(student)).ReturnsAsync(student);
            _mockMapper.Setup(mapper => mapper.Map<StudentDTO>(student)).Returns(studentDto);

            // Act
            var result = await _service.CreateStudentAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal("New", result.FirstName);
            Assert.Equal("Student", result.LastName);
        }

        [Fact]
        public async Task DeleteStudentAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var studentId = 1;
            _mockRepo.Setup(repo => repo.DeleteStudentAsync(studentId)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteStudentAsync(studentId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteStudentAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var studentId = 999;
            _mockRepo.Setup(repo => repo.DeleteStudentAsync(studentId)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteStudentAsync(studentId);

            // Assert
            Assert.False(result);
        }
    }
}