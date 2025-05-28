# HotChocolate bug reproduction

See the MachineMutation.DeleteMachineImage mutation which returns a Task.
In HC 15.1.3, the generated GraphQL schema is valid
In HC 15.1.4, the generated GraphQL schema includes types for Task, AggregateException, ++, and is invalid

Valid schema:

```graphql
# This file was generated. Do not edit manually.

schema {
  query: Query
  mutation: Mutation
  subscription: Subscription
}

"The purpose of the `cost` directive is to define a `weight` for GraphQL types, fields, and arguments. Static analysis can use these weights when calculating the overall cost of a query or response."
directive @cost(
  "The `weight` argument defines what value to add to the overall cost for every appearance, or possible appearance, of a type, field, argument, etc."
  weight: String!
) on SCALAR | OBJECT | FIELD_DEFINITION | ARGUMENT_DEFINITION | ENUM | INPUT_FIELD_DEFINITION

"The node interface is implemented by entities that have a global unique identifier."
interface Node {
  id: ID!
}

"A common interface that all user facing errors implement"
interface UserError {
  message: String!
}

type Machine {
  id: UUID!
  name: String!
  serviceAgreement: Boolean!
}

type Mutation {
  setMachineServiceAgreement(input: SetMachineServiceAgreementInput!): SetMachineServiceAgreementPayload!
}

type Query {
  machine(machineId: UUID!): Machine!
  "Fetches an object given its ID."
  node(
    "ID of the object."
    id: ID!
  ): Node
  "Lookup nodes by a list of IDs."
  nodes(
    "The list of node IDs."
    ids: [ID!]!
  ): [Node]!
}

type SetMachineServiceAgreementPayload {
  machine: Machine
}

type Subscription {
  onMachineUpdated(machineId: UUID!): Machine!
}

"Defines when a policy shall be executed."
enum ApplyPolicy {
  "After the resolver was executed."
  AFTER_RESOLVER
  "Before the resolver was executed."
  BEFORE_RESOLVER
  "The policy is applied in the validation step before the execution."
  VALIDATION
}

scalar UUID

input SetMachineServiceAgreementInput {
  machineId: UUID!
  serviceAgreement: Boolean!
}

```

Invalid shcema:

```graphql

```