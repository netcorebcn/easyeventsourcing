namespace EasyEventSourcing
{
    public static class DefaultProjection
    {
        public static string Default(string aggregateName, string streamName) => $@"
        fromCategory('{aggregateName}')
        .when({{
            $init : function(state, event)
            {{
                return {{ 
                }}
            }},
            $any: function(state, ev){{
                linkTo('{streamName}', ev);
                return state;          
            }}      
        }});";
    }
}
