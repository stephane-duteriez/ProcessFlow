using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace Site.Controls
{
    public partial class ProcessFlowControl : PortalUserControl
    {
        public string entityname = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            construct_processFlow();
        }

        private void construct_processFlow()
        {
            Guid entityid = new Guid(Request.QueryString["id"]); // get the id of the entity from the page request params
            if (entityname == null)
            {
                EntityReference adx_form_ref = Entity.GetAttributeValue<EntityReference>("adx_entityform"); //get lookup field adx_entityform from the actuel page
                if (adx_form_ref != null)
                {
                    IOrganizationService service = (IOrganizationService)XrmContext;
                    ColumnSet cols = new ColumnSet(new String[] { "adx_name", "adx_entityname" });
                    Entity adx_form = service.Retrieve(adx_form_ref.LogicalName, adx_form_ref.Id, cols); // get the Entity from the EntityReference object
                    entityname = adx_form.GetAttributeValue<string>("adx_entityname"); // the entityname of the Entity link to this page
                }
            }
            if (entityname != null)
            {
                var myEntities = XrmContext.CreateQuery(entityname).Where(t => (Guid)t[entityname + "id"] == entityid);
                Entity myEntity = (Entity)myEntities.First(); // the Entity link to this page
                Guid myProcessId = myEntity.GetAttributeValue<Guid>("processid"); // the processid link to this entity
                var processes = XrmContext.CreateQuery("workflow").Where(t => (Guid)t["workflowid"] == myProcessId);
                var process = processes.First(); // the process entity 
                Guid this_stageid = myEntity.GetAttributeValue<Guid>("stageid"); // the stage id of this entity
                string clientData_json = process.GetAttributeValue<string>("clientdata"); // the description of the process
                List<Stage> stages = get_stages(clientData_json); // extract the list of stages in order
                ProcessFlow.InnerHtml = "";
                foreach (var stage in stages)
                {
                    HtmlGenericControl p = new HtmlGenericControl("p");
                    p.Attributes["class"] = "btn btn-default";
                    p.InnerHtml = stage.label;

                    if (new Guid(stage.id) == this_stageid)
                    {
                        p.Attributes["class"] += " status";
                    }
                    ProcessFlow.Controls.Add(p);
                }
            }
        }


        private class Stage
        {
            public string id;
            public string label;

        }
        // extract the list of stages ids in order from the clientData field
        static List<Stage> get_stages(string jsonString)
        {
            List<Stage> stages = new List<Stage>();
            JObject myObejct = JObject.Parse(jsonString);
            // check it is a workflow clientdata object
            if (!((string)myObejct["__class"]).Contains("WorkflowStep"))
            {
                return stages;
            }
            foreach (JObject step in myObejct["steps"]["list"])
            {
                if (((string)step["__class"]).Contains("EntityStep"))
                {
                    foreach (JObject stage in step["steps"]["list"])
                    {
                        if (((string)stage["__class"]).Contains("StageStep"))
                        {
                            stages.Add(new Stage { id = (string)stage["stageId"], label = (string)stage["description"] });
                        }
                    }
                }
            }
            return stages;
        }
    }
}
