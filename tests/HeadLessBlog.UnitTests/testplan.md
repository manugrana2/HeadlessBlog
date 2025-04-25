## Technology Stack

- **Test Framework**: xUnit
- **Mocking Library**: Moq
- **Assertions**: FluentAssertions
- **Validator Testing**: FluentValidation.TestHelper


## Notes

- Tests mock external dependencies (repositories) to ensure isolation.
- Tests focus only on unit level (not integration).
- Data consistency and deeper database integration will be covered in separate test layers.
