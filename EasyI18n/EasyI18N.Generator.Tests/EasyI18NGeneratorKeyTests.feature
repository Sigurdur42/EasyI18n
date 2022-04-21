Feature: Safe key generation tests


Scenario: Ensure that keys are generated C# conform
	Given The manual generator is used
	When Keys need to be translated
	| Key        | Generated |
	| scope:key1 | scopekey1 |
	| Part1.Part2 | Part1Part2 |
	| Part1-Part2 | Part1Part2 |
	| Part1,Part2 | Part1Part2 |
	| Part1 Part2 | Part1Part2 |
	| Part1	Part2 | Part1Part2 |
	| Part1  Part2 | Part1Part2 |
