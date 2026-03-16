namespace BlogApi.Exceptions;

public class NotFoundException(string entityName, object id)
    : Exception($"{entityName} with ID {id} not found");
