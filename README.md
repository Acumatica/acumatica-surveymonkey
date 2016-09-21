[![Project Status](http://opensource.box.com/badges/active.svg)](http://opensource.box.com/badges)

SurveyMonkey Integration Extension for Acumatica
==================================================

An extension that allows a SurveyMonkey satisfaction survey to be used with customer support cases.  With this extension, you can do the following:
* View a list of customers with recently closed support cases
* Email links to your support feedback survey
* Retrieve batches of completed survey responses
* View and edit your customers’ responses, right inside the support case screen

### Prerequisites
* You must have a paid SurveyMonkey account (at least the Select plan)
* The SurveyMonkey survey you use must only contain the following question types:
	* Multiple Choice (with or without multiple answers, and allowing an "Other" comment box to be added)
	* Matrix / Rating Scale
	* Comment Box

Quick Start
-----------

### Installation

##### Step 1: Set up your SurveyMonkey app
1. Create a survey, using the supported question types (see Limitations)
2. Create a public app at developer.surveymonkey.com with the following settings
	* OAuth Redirect Url:	https://**yoursite.com**/**instancename**/Frames/SurveyMonkeyAuthenticator.html
	![OAuth Redirect Url](/READMEAssets/oauth_redirect_url.PNG)
	* Scopes:
	![Scopes](/READMEAssets/scopes.PNG)
	*Note:  You will be using the information in the "Credentials" section so you may want to keep this page open*

##### Step 2: Install the customization project
1. Download SurveyMonkeyIntegration.zip from this repository
2. In your Acumatica ERP instance, import SurveyMonkeyIntegration.zip as a customization project
3. Publish the customization project

##### Step 3: Create a notification template for the emails that will be sent to prompt contacts to take your survey
1. Go to the Notification Templates screen (Configuration/Email/Notification Templates)
2. Create the template.  For example, it might look like this:
![Notification Template Example](/READMEAssets/notificaton_template_example.PNG)

### Configuration
1. Go to Organization/Cash Management/Configuration/Customer Management Preferences and click the new "Case Preferences" tab
2. Set the Notification Template to the one you created
3. Set the Client ID, Client Secret, and API Key from the "Credentials" section of your SurveyMonkey app's settings page as shown:
![](/READMEAssets/app_credentials.PNG)
4. To set the Survey ID, use SurveyMonkey's /surveys endpoint (see https://developer.surveymonkey.com/api/v3/#surveys)
5. Save the changes to the page, and then click "GET ACCESS TOKEN".  
*Note: If you get an "authorization request failed" error, re-check the values you just set and make sure your OAuth Redirect Url is correct.*

Known Issues
------------
None at the moment

## Copyright and License

Copyright � `2016` `Acumatica`

This component is licensed under the MIT License, a copy of which is available online at https://github.com/Acumatica/acumatica-surveymonkey/blob/master/LICENSE.md
