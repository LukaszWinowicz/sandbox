//using Bunit;
//using MassUpdate.BlazorUI.Components.Pages;
//using MassUpdate.Core.DTOs;
//using MassUpdate.Core.Interfaces;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.FluentUI.AspNetCore.Components;
//using Moq;

//namespace MassUpdateData.Tests.bUnit;

//public class MultiRowPurchaseOrderPageTests : TestContext
//{
//    public MultiRowPurchaseOrderPageTests()
//    {
//        // Konfiguracja wspólna dla wszystkich testów w tej klasie
//        Services.AddFluentUIComponents();
//        JSInterop.Mode = JSRuntimeMode.Loose;
//    }

//    [Fact]
//    public void ValidateAll_WithMixedData_ShouldDisplayErrorsGroupedByRow()
//    {
//        // ARRANGE

//        // 1. Definiujemy "fałszywy" wynik, który ma zwrócić nasz mock serwisu.
//        //    Symulujemy błędy w wierszu 2 i 4.
//        var mockedResults = new List<RowValidationResult>
//        {
//            new RowValidationResult(2, new List<string> { "Purchase Order must be exactly 10 characters long." }),
//            new RowValidationResult(4, new List<string> { "Purchase Order 'PO2025-999' does not exist." })
//        };

//        // 2. Tworzymy mocka serwisu walidacji masowej.
//        var mockValidationService = new Mock<IMassUpdateValidationService>();

//        // 3. "Uczymy" mocka: gdy ktoś wywoła metodę Validate z dowolną listą, zwróć nasz przygotowany wynik.
//        mockValidationService
//            .Setup(s => s.Validate(It.IsAny<List<MassUpdatePurchaseOrderDto>>()))
//            .Returns(mockedResults);

//        // 4. Rejestrujemy naszego mocka w kontenerze DI bUnit.
//        Services.AddScoped<IMassUpdateValidationService>(sp => mockValidationService.Object);

//        // ACT

//        // 5. Renderujemy komponent. On sam w OnInitialized() stworzy listę 4 wierszy.
//        var cut = RenderComponent<MultiRowPurchaseOrderPage>();

//        // 6. Znajdujemy przycisk "Validate All" i symulujemy jego kliknięcie.
//        cut.Find("fluent-button:contains('Validate All')").Click();

//        // ASSERT

//        // 7. Czekamy na przerysowanie UI i sprawdzamy, czy HTML zawiera poprawne informacje.
//        cut.WaitForAssertion(() =>
//        {
//            // Sprawdzamy, czy w ogóle pojawił się nagłówek z błędami
//            Assert.Contains("Found errors in 2 row(s):", cut.Markup);

//            // Sprawdzamy, czy pojawił się błąd dla wiersza 2
//            Assert.Contains("<strong>Row 2:</strong>", cut.Markup);
//            Assert.Contains("Purchase Order must be exactly 10 characters long.", cut.Markup);

//            // Sprawdzamy, czy pojawił się błąd dla wiersza 4
//            Assert.Contains("<strong>Row 4:</strong>", cut.Markup);
//            Assert.Contains("Purchase Order 'PO2025-999' does not exist.", cut.Markup);

//            // Sprawdzamy, czy NIE pojawiły się nagłówki błędów dla poprawnych wierszy 1 i 3
//            Assert.DoesNotContain("<strong>Row 1:</strong>", cut.Markup);
//            Assert.DoesNotContain("<strong>Row 3:</strong>", cut.Markup);
//        });
//    }
//}
