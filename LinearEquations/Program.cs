using System;
using System.IO;
public abstract class SolverInterface
{
    public abstract double[] Solve(double[][] system);
}

public class Program
{
    public static void Main()
    {
        var input = File.ReadAllText("input.txt");
        var systems = input.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        var gaussianSolver = new GaussianSolver();
        var jacobiSolver = new JacobiSolver();

        foreach (var systemInput in systems)
        {
            var lines = systemInput.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var system = new double[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                system[i] = Array.ConvertAll(lines[i].Split(), double.Parse);
            }

            try
            {
                var gaussianSolution = gaussianSolver.Solve(system);
                Console.WriteLine("Gaussian method solution: " + string.Join(", ", gaussianSolution));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gaussian method error: " + ex.Message);
            }

            try
            {
                var jacobiSolution = jacobiSolver.Solve(system);
                Console.WriteLine("Jacobi method solution: " + string.Join(", ", jacobiSolution));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Jacobi method error: " + ex.Message);
            }

            Console.WriteLine();
        }
    }
}

public class GaussianSolver : SolverInterface
{
    public override double[] Solve(double[][] system)
    {
        int n = system.Length;

        foreach (var row in system)
        {
            if (row.Length != n + 1)
                throw new ArgumentException("Each equation must have exactly " + (n + 1) + " coefficients.");
        }

        double[][] augmentedMatrix = new double[n][];

        for (int i = 0; i < n; i++)
        {
            augmentedMatrix[i] = new double[n + 1];
            for (int j = 0; j < n; j++)
                augmentedMatrix[i][j] = system[i][j];
            augmentedMatrix[i][n] = system[i][n];
        }

        for (int i = 0; i < n; i++)
        {
            for (int k = i + 1; k < n; k++)
            {
                if (Math.Abs(augmentedMatrix[k][i]) > Math.Abs(augmentedMatrix[i][i]))
                {
                    var temp = augmentedMatrix[i];
                    augmentedMatrix[i] = augmentedMatrix[k];
                    augmentedMatrix[k] = temp;
                }
            }

            for (int k = i + 1; k < n; k++)
            {
                double factor = augmentedMatrix[k][i] / augmentedMatrix[i][i];
                for (int j = i; j <= n; j++)
                    augmentedMatrix[k][j] -= factor * augmentedMatrix[i][j];
            }
        }

        double[] result = new double[n];
        for (int i = n - 1; i >= 0; i--)
        {
            result[i] = augmentedMatrix[i][n] / augmentedMatrix[i][i];
            for (int k = i - 1; k >= 0; k--)
                augmentedMatrix[k][n] -= augmentedMatrix[k][i] * result[i];
        }

        return result;
    }
}

public class JacobiSolver : SolverInterface
{
    public override double[] Solve(double[][] system)
    {
        int n = system.Length;

        foreach (var row in system)
        {
            if (row.Length != n + 1)
                throw new ArgumentException("Each equation must have exactly " + (n + 1) + " coefficients.");
        }

        if (n == 0 || system[0].Length != n + 1)
            throw new ArgumentException("Invalid system of equations.");

        double[] x = new double[n];
        double[] xNew = new double[n];
        double tolerance = 1e-10;
        int maxIterations = 1000;

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                        sum += system[i][j] * x[j];
                }
                xNew[i] = (system[i][n] - sum) / system[i][i];
            }

            double maxDiff = 0;
            for (int i = 0; i < n; i++)
            {
                maxDiff = Math.Max(maxDiff, Math.Abs(xNew[i] - x[i]));
            }

            if (maxDiff < tolerance)
            {
                return xNew;
            }

            Array.Copy(xNew, x, n);
        }

        throw new Exception("Jacobi method did not converge within the maximum number of iterations.");
    }
}
