using System;


namespace TestLinearEquations1
{
    [TestClass]
    public class SolverTests
    {
        [TestMethod]
        public void GaussianSolver_SolvesSimpleSystem()
        {
            // Arrange
            var solver = new GaussianSolver();
            double[][] system = new double[][]
            {
                new double[] { 2, 1, 1, 4 }, // 2x + y + z = 4
                new double[] { 1, -1, 0, 1 }, // x - y = 1
                new double[] { 1, 2, 3, 9 }   // x + 2y + 3z = 9
            };

            // Act
            var solution = solver.Solve(system);

            // Assert
            Assert.AreEqual(2, solution[0], 1e-5);
            Assert.AreEqual(1, solution[1], 1e-5);
            Assert.AreEqual(1, solution[2], 1e-5);
        }

        [TestMethod]
        public void JacobiSolver_SolvesSimpleSystem()
        {
            // Arrange
            var solver = new JacobiSolver();
            double[][] system = new double[][]
            {
                new double[] { 4, -1, 0, 3 }, // 4x - y = 3
                new double[] { -1, 4, -1, 3 }, // -x + 4y - z = 3
                new double[] { 0, -1, 4, 3 }   // -y + 4z = 3
            };

            // Act
            var solution = solver.Solve(system);

            // Assert
            Assert.AreEqual(1, solution[0], 1e-5);
            Assert.AreEqual(1, solution[1], 1e-5);
            Assert.AreEqual(1, solution[2], 1e-5);
        }

        [TestMethod]
        public void JacobiSolver_ThrowsExceptionOnInvalidSystem()
        {
            // Arrange
            var solver = new JacobiSolver();
            double[][] invalidSystem = new double[][]
            {
                new double[] { 4, -1 }, // Invalid: not enough coefficients for an equation
                new double[] { -1, 4 }
            };

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => solver.Solve(invalidSystem));
            Assert.AreEqual("Invalid system of equations.", exception.Message);
        }

        [TestMethod]
        public void GaussianSolver_ThrowsExceptionOnSingularMatrix()
        {
            // Arrange
            var solver = new GaussianSolver();
            double[][] singularSystem = new double[][]
            {
                new double[] { 1, 2, -1, -8 },
                new double[] { 2, 4, -2, -16 },
                new double[] { -3, -6, 3, 24 }
            };

            // Act & Assert (Expecting an exception due to singular matrix)
            Assert.ThrowsException<DivideByZeroException>(() => solver.Solve(singularSystem));
        }
    }
}