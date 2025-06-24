using MassUpdateData.Builders;
using MassUpdateData.Handlers;
using MassUpdateData.Services;
using MassUpdateData.Validators;
using Moq;
using System.Reflection;

namespace MassUpdateData.Tests.IntegrationTests;

public class ValidationChainBuilderTests
{
    [Fact]
    public void Build_ShouldCreateCorrectlyLinkedChain()
    {
        // ARRANGE
        // 1. Tworzymy mock serwisu danych
        var mockDataService = new Mock<IOrderDataService>();

        // 2. Tworzymy instancję Budowniczego, którego będziemy testować.
        var builder = new ValidationChainBuilder();

        // ACT
        // 3. Używamy Budowniczego do złożenia łańcucha z trzech ogniw.
        var firstHandler = builder
            .WithNotEmptyCheck(dto => "dummy", "TestField") // Pierwsze ogniwo
            .WithStringLengthCheck(dto => "dummy", 10, "TestField") // Drugie ogniwo
            .WithExistenceCheck<string>(dto => "dummy", mockDataService.Object.OrderExists, "TestField") // Trzecie ogniwo
            .Build();

        // ASSERT
        // 4. Sprawdzamy, czy łańcuch został poprawnie połączony za pomocą refleksji.

        // Sprawdzenie 1: Czy pierwszy element jest właściwego typu?
        Assert.IsType<NotEmptyValidator>(firstHandler);

        // Sprawdzenie 2: Czy pierwsze ogniwo (NotEmpty) wskazuje na drugie (StringLength)?
        var secondHandler = GetNextHandler(firstHandler);
        Assert.NotNull(secondHandler);
        Assert.IsType<StringLengthValidator>(secondHandler);

        // Sprawdzenie 3: Czy drugie ogniwo (StringLength) wskazuje na trzecie (Existence)?
        var thirdHandler = GetNextHandler(secondHandler!);
        Assert.NotNull(thirdHandler);
        Assert.IsType<ExistenceValidator<string>>(thirdHandler);

        // Sprawdzenie 4: Czy trzecie ogniwo jest ostatnie (jego _nextHandler jest nullem)?
        var endOfChain = GetNextHandler(thirdHandler!);
        Assert.Null(endOfChain);
    }

    // Metoda pomocnicza używająca refleksji do pobrania wartości chronionego pola _nextHandler
    private IValidationHandler? GetNextHandler(IValidationHandler handler)
    {
        // Znajdź pole o nazwie "_nextHandler" w typie obiektu 'handler'
        var fieldInfo = handler.GetType().BaseType?.GetField("_nextHandler", BindingFlags.NonPublic | BindingFlags.Instance);

        // Pobierz wartość tego pola dla konkretnej instancji 'handler'
        return fieldInfo?.GetValue(handler) as IValidationHandler;
    }

}
