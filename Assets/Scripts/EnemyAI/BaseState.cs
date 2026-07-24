using UnityEngine;

namespace EnemyAI.StateMachine
{
    /// <summary>
    /// Abstract class useful for stroing enemy states for an enemy state machine
    /// </summary>
    public abstract class BaseState
    {
        public Enemy enemy;                                 // the enemy this state is attached to
        public EnemyStateMachine stateMachine;   // the state machine this state is attached to

        /// <summary>
        /// Function called when a state is entered
        /// </summary>
        public abstract void Enter();
        /// <summary>
        /// Function called when a state is performed in Update
        /// </summary>
        public abstract void Perform();
        /// <summary>
        /// Function called when a state is exited
        /// </summary>
        public abstract void Exit();
    }

}
