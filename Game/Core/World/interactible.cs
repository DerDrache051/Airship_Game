using Godot;

public interface IIntractable {

    public void interact(Node interaction_source);

    public void endInteraction();
}