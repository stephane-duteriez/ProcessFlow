# Process Flow:
Display a process flow in adxPortal as in the crm
## Goals :
I needed to show the stage name of an entity to a contact in adxPortal. A first experiment was done using the liquid template but I had to hardcore the name of the stages in the code. After analyzing the structure of the process entity and stage entity in the crm I was able to extract all the needed information.
## Files:
* Custom Controller: ProcesseFlowControl.ascx, ProcessFlowControl.ascx.cs and ProcessFlowControl.ascx.designer.cs, should be place in the Controls folder (better in your custom Areas).
* Css: Process_flow.css in the css folder or your custom css folder in your Area. I customized a css found http://bootsnipp.com/snippets/featured/triangle-breadcrumbs-arrows.  
* Example of the code in the aspx Page: WebPage.aspx 
## Magic:
I use the clientData field from the process entity to retrieve the order of the stages. It is a json string containing the description of the process. I actually retrieve directly the description of the process but in a multilinguals site it would be possible to select the description by language code by digging more inside the json.  
:warning: This clientData field is not documented and future update of xrm may change it.

## To go further:
It wasn’t necessary for my project to allowed the portal contact to make change of the process flow, which are made on the xrm platform. But it could be implemented using JavaScript. 

