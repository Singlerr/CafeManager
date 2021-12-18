using System.Collections.Generic;

namespace Conditions
{
    internal class ConditionParentPipeline
    {
        private int _currentStep;

        private List<ConditionPipeline> _pipelines;

        public Condition Condition;

        public ConditionParentPipeline(Condition condition)
        {
            Condition = condition;
            _currentStep = 0;
            _pipelines = new List<ConditionPipeline>();
        }

        public ConditionParentPipeline()
        {
        }

        public void AddLast(ConditionPipeline pipeline)
        {
            _pipelines.Add(pipeline);
        }

        public void AddFirst(ConditionPipeline pipeline)
        {
            var list = new List<ConditionPipeline>();
            list.Add(pipeline);
            list.AddRange(_pipelines);

            _pipelines = new List<ConditionPipeline>(list);
            list = null;
        }

        public bool CheckConditions()
        {
            var stacked = false;
            while (HasNext())
            {
                stacked = _pipelines[_currentStep].DoFilter(_currentStep, Condition, stacked);
                _currentStep++;
            }

            return stacked;
        }

        private bool HasNext()
        {
            return _pipelines.Count < _currentStep;
        }

        public void Reset()
        {
            _currentStep = 0;
        }
    }
}