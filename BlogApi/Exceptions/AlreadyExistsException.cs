namespace BlogApi.Exceptions;

public class AlreadyExistsException(string entityName, string field)
    : Exception($"{entityName} with {field} already exists");
