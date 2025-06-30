//using MassUpdate.Core.DTOs;
//using MassUpdate.Core.Handlers;
//using MassUpdate.Core.Interfaces;
//using MassUpdate.Core.Validators.Components;
//using Moq;

//namespace MassUpdateData.Tests.UnitTests;

//public class CombinationValidatorTests
//{
//    [Fact]
//    public void Validate_ShouldAddError_WhenCombinationCheckReturnsFalse()
//    {
//        // 1. Mock serwisu danych nie jest tutaj kluczowy, ale potrzebujemy obiektu,
//        //    który implementuje interfejs, aby przekazać go do walidatora.
//        var mockDataService = new Mock<IOrderDataService>();

//        // 2. Definiujemy nasz "przepis na walidację". W tym teście chcemy,
//        //    aby zawsze zwracał `false`, symulując nieudaną walidację.
//        Func<MassUpdatePurchaseOrderDto, IOrderDataService, bool> failingCheck =
//            (dto, service) => false;

//        // 3. Tworzymy instancję naszego generycznego walidatora.
//        var validator = new CombinationValidator<MassUpdatePurchaseOrderDto, IOrderDataService>(
//            mockDataService.Object,
//            failingCheck,
//            "Combination is invalid."
//        );

//        var request = new ValidationRequest(new MassUpdatePurchaseOrderDto
//        {
//            // Musimy podać wartość dla każdego wymaganego pola.
//            // Konkretne wartości nie mają znaczenia dla tego testu.
//            PurchaseOrder = "DUMMY_PO_FOR_TEST",
//            LineNumber = 1,
//            Sequence = 1,
//            ReceiptDate = DateTime.Now
//        });

//        // ACT
//        validator.Validate(request);

//        // ASSERT
//        Assert.Single(request.ValidationErrors); // Oczekujemy dokładnie jednego błędu.
//        Assert.Equal("Combination is invalid.", request.ValidationErrors[0]); // Sprawdzamy treść błędu.
//    }

//    [Fact]
//    public void Validate_ShouldNotAddError_WhenCombinationCheckReturnsTrue()
//    {
//        // 1. Mock serwisu danych nie jest tutaj kluczowy, ale potrzebujemy obiektu,
//        //    który implementuje interfejs, aby przekazać go do walidatora.
//        var mockDataService = new Mock<IOrderDataService>();

//        // 2. Definiujemy nasz "przepis na walidację". W tym teście chcemy,
//        //    aby zawsze zwracał `false`, symulując nieudaną walidację.
//        Func<MassUpdatePurchaseOrderDto, IOrderDataService, bool> passingCheck =
//            (dto, service) => true;

//        // 3. Tworzymy mocka NASTĘPNEGO ogniwa w łańcuchu, aby sprawdzić, czy zostanie wywołany.
//        var mockNextHandler = new Mock<IValidationHandler>();

//        // 4. Tworzymy instancję naszego generycznego walidatora.
//        var validator = new CombinationValidator<MassUpdatePurchaseOrderDto, IOrderDataService>(
//            mockDataService.Object,
//            passingCheck,
//            "Combination is invalid."
//        );

//        var request = new ValidationRequest(new MassUpdatePurchaseOrderDto
//        {
//            // Musimy podać wartość dla każdego wymaganego pola.
//            // Konkretne wartości nie mają znaczenia dla tego testu.
//            PurchaseOrder = "DUMMY_PO_FOR_TEST",
//            LineNumber = 1,
//            Sequence = 1,
//            ReceiptDate = DateTime.Now
//        });

//        // ACT
//        validator.Validate(request);

//        // ASSERT
//        // 1. Sprawdzamy, czy nie został dodany żaden błąd
//        Assert.Empty(request.ValidationErrors);
//    }
//}