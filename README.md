# HotChocolate bug reproduction

See the MachineMutation.DeleteMachineImage mutation which returns a Task.
In HC 15.1.3, the generated GraphQL schema is valid
In HC 15.1.4, the generated GraphQL schema includes types for Task, AggregateException, ++, and is invalid:

Schema validation error message:
"DictionaryEntry" must define one or more fields

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

interface IDictionary {
  enumerator: IDictionaryEnumerator!
  isFixedSize: Boolean!
  isReadOnly: Boolean!
}

interface IDictionaryEnumerator {
  entry: DictionaryEntry!
}

"The node interface is implemented by entities that have a global unique identifier."
interface Node {
  id: ID!
}

"A common interface that all user facing errors implement"
interface UserError {
  message: String!
}

type AggregateException {
  baseException: Exception!
  data: IDictionary!
  flatten: AggregateException!
  hResult: Int!
  helpLink: String
  innerException: Exception
  innerExceptions: [Exception!]!
  message: String!
  source: String
  stackTrace: String
}

type ConfiguredTaskAwaitable {
  awaiter: ConfiguredTaskAwaiter!
}

type ConfiguredTaskAwaiter {
  isCompleted: Boolean!
}

type DeleteMachineImagePayload {
  task: Task
}

type DictionaryEntry

type Exception {
  baseException: Exception!
  data: IDictionary!
  hResult: Int!
  helpLink: String
  innerException: Exception
  message: String!
  source: String
  stackTrace: String
}

type Machine {
  id: UUID!
  name: String!
  serviceAgreement: Boolean!
}

type Mutation {
  deleteMachineImage(input: DeleteMachineImageInput!): DeleteMachineImagePayload!
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

type Task {
  awaiter: TaskAwaiter!
  configureAwait(continueOnCapturedContext: Boolean!): ConfiguredTaskAwaitable!
  creationOptions: TaskCreationOptions!
  exception: AggregateException
  id: Int!
  isCanceled: Boolean!
  isCompleted: Boolean!
  isCompletedSuccessfully: Boolean!
  isFaulted: Boolean!
  status: TaskStatus!
  wait(timeout: TimeSpan!): Boolean!
}

type TaskAwaiter {
  isCompleted: Boolean!
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

enum TaskCreationOptions {
  ATTACHED_TO_PARENT
  DENY_CHILD_ATTACH
  HIDE_SCHEDULER
  LONG_RUNNING
  NONE
  PREFER_FAIRNESS
  RUN_CONTINUATIONS_ASYNCHRONOUSLY
}

enum TaskStatus {
  CANCELED
  CREATED
  FAULTED
  RAN_TO_COMPLETION
  RUNNING
  WAITING_FOR_ACTIVATION
  WAITING_FOR_CHILDREN_TO_COMPLETE
  WAITING_TO_RUN
}

"The `TimeSpan` scalar represents an ISO-8601 compliant duration type."
scalar TimeSpan

scalar UUID

input DeleteMachineImageInput {
  machineId: UUID!
}

input SetMachineServiceAgreementInput {
  machineId: UUID!
  serviceAgreement: Boolean!
}

```