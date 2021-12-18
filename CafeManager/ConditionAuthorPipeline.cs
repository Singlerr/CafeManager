using System;
using Conditions;
using Pipelines;

namespace CafeManager
{
    internal class ConditionAuthorPipeline : ConditionPipeline
    {
        private readonly string _author;
        public readonly ConditionOperationType OperationType;

        public ConditionAuthorPipeline(ParentPipeline pipeline, string author, ConditionOperationType operationType) :
            base(pipeline)
        {
            _author = author;
            OperationType = operationType;
        }

        public override bool DoFilter(int step, Condition cond, bool stacked)
        {
            return cond.Author.Equals(_author, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}