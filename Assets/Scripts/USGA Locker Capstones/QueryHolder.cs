using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoshKery.GenericUI.GraphQL
{
    public class QueryHolder : MonoBehaviour
    {
        public string url;
        public string authToken;
        public string operationName;
        [Multiline]
        public string variables;
        [Multiline]
        public string query;

        public WWWForm GetPostForm()
        {
            WWWForm form = new WWWForm();
            if (!string.IsNullOrEmpty(query))
                form.AddField("query", query);
            if (!string.IsNullOrEmpty(operationName))
                form.AddField("operationName", operationName);
            if (!string.IsNullOrEmpty(variables))
                form.AddField("variables", variables);

            return form;
        }

        public string GetPostString()
        {
            return string.Format("{{query:{0},operationName:{1},variables{2}}}", query, operationName, variables);
        }
    }
}

