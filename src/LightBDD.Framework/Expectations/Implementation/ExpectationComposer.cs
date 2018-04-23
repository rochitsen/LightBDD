﻿namespace LightBDD.Framework.Expectations.Implementation
{
    internal class ExpectationComposer : IExpectationComposer
    {
        private bool _isNot = false;
        public IExpectationComposer Not
        {
            get
            {
                _isNot = !_isNot;
                return this;
            }
        }

        public IExpectationComposer<T> For<T>()
        {
            var composer = new ExpectationComposer<T>();
            if (_isNot)
                return composer.Not;
            return composer;
        }
    }

    internal class ExpectationComposer<T> : IExpectationComposer<T>
    {
        private bool _isNot = false;
        public IExpectationComposer<T> Not
        {
            get
            {
                _isNot = !_isNot;
                return this;
            }
        }
        public Expectation<T> Compose(Expectation<T> expectation)
        {
            return _isNot ? new NotExpectation<T>(expectation) : expectation;
        }
    }
}