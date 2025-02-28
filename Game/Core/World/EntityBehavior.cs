using Godot;
public abstract partial class EntityBehavior:Node{
    public abstract Vector2 getDesiredVelocity();//where should the entity move
    public abstract bool shouldJump();//should the entity jump. not needed on flying entities
    public abstract bool isFinished();//is the behavior finished. always return true if the action should not block other actions
    public abstract bool isPossible();//can the behavoir be started
    public abstract void doBehavior();//runs the behavior
}