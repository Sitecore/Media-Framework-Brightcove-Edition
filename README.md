
# Brightcove Connector for Sitecore 

Developer Setup

1) Install Sitecore 9.0 via instructions from the official Sitecore dev site
 
2) Install the Brightcove connector via the package in Data/packages/Brightcove Media Framework-9.0.zip. This now includes media framework. You can also put the xml file in the Sitecore packages directory in order to generate/modify the package.

3) Modify the Brightcove .Extended config by replacing placeholder values and remove the disabled extension

4) Modify App_Config/Includes/Unicorn/Unicorn.Brightcove.config to point to the Serialization directory in your source directory. Keep in mind you may need to change app pool identity or give permissions to the Serialization folder.

5) Run the SQL script in the src/Data/ folder inside the solution on the reporting DB

6) Perform a smart publish of the entire site

Then, you can use this solution and publish over your website as usual.

# License

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.

You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
