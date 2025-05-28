# HotChocolate bug reproduction

See the MachineMutation.DeleteMachineImage mutation which returns a Task.
In HC 15.1.3, the generated GraphQL schema is valid
In HC 15.1.4, the generated GraphQL schema includes types for Task, AggregateException, ++, and is invalid
