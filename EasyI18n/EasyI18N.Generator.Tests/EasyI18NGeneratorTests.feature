Feature: Verify the generator features


Scenario: Verify simple generated code
	Given The manual generator is used
	When Code is generated for 'LocalizeTestData.xml'
	And Code has been generated successfully

	Then Code is generated as in 'LocalizeTestData.g.cs'
	Then The view model is generated as in 'LocalizeTestDataViewModel.g.cs'

	Then Code file name is generated as 'LocalizeTestData.g.cs'
	Then View model file name is generated as 'LocalizeTestDataViewModel.g.cs'		
