using Bunit;
using MassUpdate.BlazorUI.Components.Pages;
using MassUpdate.Core.DTOs;
using MassUpdate.Core.Validators.Orchestrators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Moq;

namespace MassUpdateData.Tests;

public class SinglePurchaseOrderPageTests : TestContext
{
    public SinglePurchaseOrderPageTests()
    {
        // Rejestrujemy wszystkie niezbędne serwisy Fluent UI w środowisku testowym bUnit.
        // To jest odpowiednik tego, co robimy w Program.cs w głównej aplikacji.
        Services.AddFluentUIComponents();

        // Mówimy bUnit: "Jeśli jakikolwiek komponent poprosi o załadowanie
        // modułu JavaScript, którego jawnie nie skonfigurowałem,
        // zignoruj to i udawaj, że załadowałeś pusty moduł."
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void HandleValidation_ShouldDisplayErrors_WhenValidationFails()
    {
        // ARRANGE
        // 1. Definiujemy, jakie błędy ma zwrócić nasz "fałszywy" walidator.
        var expectedErrors = new List<string> { "PO number is too short.", "Date is in the past." };

        // 2. Tworzymy mocka naszego głównego walidatora.
        var mockValidator = new Mock<IPurchaseOrderMassUpdateValidator>();

        // 3. "Uczymy" go, że gdy jego metoda Validate zostanie wywołana, ma zwrócić naszą listę błędów.
        // Używamy It.IsAny<>() aby mock reagował na dowolny obiekt DTO.
        mockValidator.Setup(v => v.Validate(It.IsAny<MassUpdatePurchaseOrderDto>())).Returns(expectedErrors);

        // 4. Rejestrujemy zamockowany walidator w kontenerze DI biblioteki bUnit.
        Services.AddScoped<IPurchaseOrderMassUpdateValidator>(sp => mockValidator.Object);

        // ACT
        // 5. Renderujemy nasz komponent w pamięci.
        var cut = RenderComponent<SinglePurchaseOrderPage>(); // "cut" = Component Under Test

        // 6. Znajdujemy przycisk "Validate" w wyrenderowanym HTML-u i symulujemy jego kliknięcie.
        cut.Find("form").Submit();

        // ASSERT
        // 7. Sprawdzamy, czy wyrenderowany kod HTML (Markup) zawiera teraz nasze komunikaty o błędach.
        // Używamy WaitForAssertion, aby dać Blazorowi czas na ponowne wyrenderowanie
        // komponentu po zmianie jego stanu, która nastąpiła w wyniku kliknięcia przycisku.
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Validation Errors:", cut.Markup);
            Assert.Contains("PO number is too short.", cut.Markup);
            Assert.Contains("Date is in the past.", cut.Markup);
        });

        // 8. Weryfikacja mocka może pozostać na zewnątrz, bo nie zależy od renderowania HTML.
        mockValidator.Verify(v => v.Validate(It.IsAny<MassUpdatePurchaseOrderDto>()), Times.Once);
    }
}
