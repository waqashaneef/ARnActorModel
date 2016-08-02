﻿/*****************************************************************************
		               ARnActor Actor Model Library .Net
     
	 Copyright (C) {2015}  {ARn/SyndARn} 
 
 
     This program is free software; you can redistribute it and/or modify 
     it under the terms of the GNU General Public License as published by 
     the Free Software Foundation; either version 2 of the License, or 
     (at your option) any later version. 
 
 
     This program is distributed in the hope that it will be useful, 
     but WITHOUT ANY WARRANTY; without even the implied warranty of 
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
     GNU General Public License for more details. 
 
 
     You should have received a copy of the GNU General Public License along 
     with this program; if not, write to the Free Software Foundation, Inc., 
     51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA. 
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Actor.Base
{
    /// <summary>
    /// Behaviors holds many behaviors.
    /// Behaviors are actor brain.
    /// Behaviors when null in an actor means this actor is dead (it can't change anymore his own behavior from this point)
    /// </summary>
    public class Behaviors
    {
        private List<IBehavior> fList = new List<IBehavior>();

        public BaseActor LinkedActor { get; private set; }

        public Behaviors()
        {
        }

        public IEnumerable<IBehavior> AllBehaviors()
        {
            return fList.ToList(); // force clone
        }

        public void LinkToActor(BaseActor anActor)
        {
            LinkedActor = anActor;
        }

        public bool FindBehavior(IBehavior aBehavior)
        {
            return fList.Contains(aBehavior);
        }

        public void AddBehavior(IBehavior aBehavior)
        {
            if (aBehavior != null)
            {
                aBehavior.LinkBehaviors(this);
                fList.Add(aBehavior);
            }
        }
        public void RemoveBehavior(IBehavior aBehavior)
        {
            CheckArg.Behavior(aBehavior);
            aBehavior.LinkBehaviors(null);
            fList.Remove(aBehavior);
        }

    }

    public class Behavior<T1, T2> : IBehavior<T1, T2>, IBehavior
    {
        public Func<T1, T2, bool> Pattern { get; protected set; }
        public Action<T1, T2> Apply { get; protected set; }
        public TaskCompletionSource<Tuple<T1, T2>> Completion { get; protected set; }
        public TaskCompletionSource<object> StandardCompletion
        {
            get
            {
                return Completion as TaskCompletionSource<Object>;
            }
        }

        private Behaviors fLinkedBehaviors;

        public BaseActor LinkedActor
        {
            get
            {
                return fLinkedBehaviors.LinkedActor;
            }
        }

        public Behaviors LinkedTo
        {
            get
            {
                return fLinkedBehaviors;
            }
        }

        public void LinkBehaviors(Behaviors someBehaviors)
        {
            fLinkedBehaviors = someBehaviors;
        }

        public Behavior(Func<T1, T2, Boolean> aPattern, Action<T1, T2> anApply)
        {
            Pattern = aPattern;
            Apply = anApply;
            Completion = null;
        }

        public Behavior(Func<T1, T2, Boolean> aPattern, TaskCompletionSource<Tuple<T1, T2>> aCompletion)
        {
            Pattern = aPattern;
            Apply = null;
            Completion = aCompletion;
        }

        public Behavior()
        {
        }

        public Func<T2, Boolean> DefaultPattern()
        {
            return t => { return t is T2; };
        }

        public Behavior(Action<T1, T2> anApply)
        {
            Pattern = (a, t) => { return a is T1 && t is T2; };
            Apply = anApply;
            Completion = null;
        }

        public Boolean StandardPattern(object aT)
        {
            if (Pattern == null)
                return false;
            Tuple<T1, T2> tupleT = aT as Tuple<T1, T2>;
            if (tupleT != null)
                return Pattern(tupleT.Item1, tupleT.Item2);
            else return false;
        }

        public void StandardApply(object aT)
        {
            if (Apply != null)
            {
                Tuple<T1, T2> tupleT = (Tuple<T1, T2>)aT;
                Apply(tupleT.Item1, tupleT.Item2);
            }
        }
    }

    public class Behavior<T1, T2, T3> : IBehavior<T1, T2, T3>, IBehavior
    {
        public Func<T1, T2, T3, bool> Pattern { get; protected set; }
        public Action<T1, T2, T3> Apply { get; protected set; }
        public TaskCompletionSource<Tuple<T1, T2, T3>> Completion { get; protected set; }
        public TaskCompletionSource<object> StandardCompletion
        {
            get
            {
                return Completion as TaskCompletionSource<object>;
            }
        }

        private Behaviors fLinkedBehaviors;

        public BaseActor LinkedActor
        {
            get
            {
                return fLinkedBehaviors.LinkedActor;
            }
        }

        public Behaviors LinkedTo
        {
            get
            {
                return fLinkedBehaviors;
            }
        }

        public void LinkBehaviors(Behaviors someBehaviors)
        {
            fLinkedBehaviors = someBehaviors;
        }

        public Behavior(Func<T1, T2, T3, bool> aPattern, Action<T1, T2, T3> anApply)
        {
            Pattern = aPattern;
            Apply = anApply;
            Completion = null;
        }

        public Behavior(Func<T1, T2, T3, bool> aPattern, TaskCompletionSource<Tuple<T1, T2, T3>> aCompletion)
        {
            Pattern = aPattern;
            Apply = null;
            Completion = aCompletion;
        }

        public Behavior()
        {
        }

        public Func<T1, Boolean> DefaultPattern()
        {
            return t => { return t is T1; };
        }

        public Behavior(Action<T1, T2, T3> anApply)
        {
            Pattern = (o, d, a) => { return o is T1 && d is T2 && a is T3; };
            Apply = anApply;
            Completion = null;
        }

        public bool StandardPattern(object aT)
        {
            if (Pattern == null)
                return false;
            Tuple<T1, T2, T3> tupleT = aT as Tuple<T1, T2, T3>;
            if (tupleT != null)
                return Pattern(tupleT.Item1, tupleT.Item2, tupleT.Item3);
            else return false;
        }

        public void StandardApply(object aT)
        {
            if (Apply != null)
            {
                Tuple<T1, T2, T3> tupleT = (Tuple<T1, T2, T3>)aT;
                Apply(tupleT.Item1, tupleT.Item2, tupleT.Item3);
            }
        }
    }

    /// <summary>
    /// bhvBehavior
    /// A behavior is describe with two properties : Pattern and Apply.
    /// At message reception, it's tested against each Pattern and if it succeeded, 
    /// the Apply is invoke with this message as parameter.
    /// Patterns order can be relevant in this process.
    /// Type is the acq of the message to be send to this behavior
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Behavior<T> : IBehavior<T>, IBehavior
    {
        public Func<T, bool> Pattern { get; protected set; }
        public Action<T> Apply { get; protected set; }
        public TaskCompletionSource<T> Completion { get; protected set; }

        public TaskCompletionSource<object> StandardCompletion
        {
            get
            {
                return Completion as TaskCompletionSource<object>;
            }
        }

        private Behaviors fLinkedBehaviors;

        public BaseActor LinkedActor
        {
            get
            {
                return fLinkedBehaviors != null ? fLinkedBehaviors.LinkedActor : null;
            }
        }

        public Behaviors LinkedTo
        {
            get
            {
                return fLinkedBehaviors;
            }
        }

        public void LinkBehaviors(Behaviors someBehaviors)
        {
            fLinkedBehaviors = someBehaviors;
        }

        public Behavior(Func<T, bool> aPattern, Action<T> anApply)
        {
            Pattern = aPattern;
            Apply = anApply;
            Completion = null;
        }

        public Behavior(Func<T, bool> aPattern, TaskCompletionSource<T> aCompletion)
        {
            Pattern = aPattern;
            Apply = null;
            Completion = aCompletion;
        }

        public Behavior()
        {
        }

        public Func<T, bool> DefaultPattern()
        {
            return t => { return t is T; };
        }

        public Behavior(Action<T> anApply)
        {
            Pattern = t => { return t is T; };
            Apply = anApply;
            Completion = null;
        }

        public bool StandardPattern(object aT)
        {
            if (Pattern == null)
            {
                return false;
            }
            if (aT is T)
            {
                return Pattern((T)aT);
            }
            return false;
        }

        public void StandardApply(object aT)
        {
            Apply?.Invoke((T)aT);
        }
    }

    public class Behavior<T1, T2, T3, T4> : IBehavior<T1, T2, T3, T4>, IBehavior
    {
        public Func<T1, T2, T3, T4, bool> Pattern { get; protected set; }
        public Action<T1, T2, T3,T4> Apply { get; protected set; }
        public TaskCompletionSource<Tuple<T1, T2, T3, T4>> Completion { get; protected set; }
        public TaskCompletionSource<object> StandardCompletion
        {
            get
            {
                return Completion as TaskCompletionSource<object>;
            }
        }

        private Behaviors fLinkedBehaviors;

        public BaseActor LinkedActor
        {
            get
            {
                return fLinkedBehaviors.LinkedActor;
            }
        }

        public Behaviors LinkedTo
        {
            get
            {
                return fLinkedBehaviors;
            }
        }

        public void LinkBehaviors(Behaviors someBehaviors)
        {
            fLinkedBehaviors = someBehaviors;
        }

        public Behavior(Func<T1, T2, T3, T4, bool> aPattern, Action<T1, T2, T3, T4> anApply)
        {
            Pattern = aPattern;
            Apply = anApply;
            Completion = null;
        }

        public Behavior(Func<T1, T2, T3, T4, bool> aPattern, TaskCompletionSource<Tuple<T1, T2, T3, T4>> aCompletion)
        {
            Pattern = aPattern;
            Apply = null;
            Completion = aCompletion;
        }

        public Behavior()
        {
        }

        public Func<T1, T2, T3, T4, Boolean> DefaultPattern()
        {
            return (o, d, a, r) => { return o is T1 && d is T2 && a is T3 && r is T4; };
        }

        public Behavior(Action<T1, T2, T3, T4> anApply)
        {
            Pattern = (o, d, a, r) => { return o is T1 && d is T2 && a is T3 && r is T4; };
            Apply = anApply;
            Completion = null;
        }

        public Boolean StandardPattern(Object aT)
        {
            if (Pattern == null)
                return false;
            Tuple<T1, T2, T3, T4> tupleT = aT as Tuple<T1, T2, T3, T4>;
            if (tupleT != null)
                return Pattern(tupleT.Item1, tupleT.Item2, tupleT.Item3, tupleT.Item4);
            else return false;
        }

        public void StandardApply(Object aT)
        {
            if (Apply != null)
            {
                Tuple<T1, T2, T3, T4> tupleT = (Tuple<T1, T2, T3, T4>)aT;
                Apply(tupleT.Item1, tupleT.Item2, tupleT.Item3, tupleT.Item4);
            }
        }
    }


}
