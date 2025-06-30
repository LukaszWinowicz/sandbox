using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;
using MassUpdate.Core.Validators.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MassUpdate.Core.Builders
{
    public class ValidationChainBuilder
    {
        private IValidationHandler? _head;
        private IValidationHandler? _tail;

        private void AddHandler(IValidationHandler handler)
        {
            if (_head == null)
            {
                _head = handler;
                _tail = handler;
            }
            else
            {
                _tail!.SetNext(handler);
                _tail = handler;
            }
        }

        public ValidationChainBuilder WithNotEmptyCheck(Func<MassUpdateDto, string?> valueProvider, string fieldName)
        {
            AddHandler(new NotEmptyValidator(valueProvider, fieldName));
            return this;
        }

        public ValidationChainBuilder WithStringLengthCheck(Func<MassUpdateDto, string?> valueProvider, int length, string fieldName)
        {
            AddHandler(new StringLengthValidator(valueProvider, length, fieldName));
            return this;
        }

        // POPRAWKA 1: Ta metoda teraz poprawnie akceptuje Func<T, Task<bool>>
        public ValidationChainBuilder WithExistenceCheck<T>(Func<MassUpdateDto, T> valueProvider, Func<T, Task<bool>> existenceCheckFunc, string fieldName)
        {
            AddHandler(new ExistenceValidator<T>(valueProvider, existenceCheckFunc, fieldName));
            return this;
        }

        // POPRAWKA 2: Ta metoda nie ma już błędnego ograniczenia "where T : class"
        public ValidationChainBuilder WithNotNullCheck<T>(Func<MassUpdateDto, T?> valueProvider, string fieldName)
        {
            AddHandler(new NotNullValidator<T>(valueProvider, fieldName));
            return this;
        }

        public ValidationChainBuilder WithFutureDateCheck(Func<MassUpdateDto, DateTime?> dateProvider, string fieldName)
        {
            AddHandler(new FutureDateValidator(dateProvider, fieldName));
            return this;
        }

        public ValidationChainBuilder WithMinValueCheck<T>(Func<MassUpdateDto, T> valueProvider, T minValue, string fieldName) where T : IComparable<T>
        {
            AddHandler(new MinValueValidator<T>(valueProvider, minValue, fieldName));
            return this;
        }

        public IValidationHandler Build()
        {
            if (_head == null)
            {
                throw new InvalidOperationException("Cannot build an empty validation chain.");
            }
            return _head;
        }
    }
}