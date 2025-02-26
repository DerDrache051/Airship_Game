using Godot;
public enum DamageTypes{
    Impact=0,
    Explosion=1,//Kaboom?
    Piercing=2,//bullets
    Fire=3,
    Magic=4,
    Electric=5,
    Poison=6,
    Heal=7,//Used for healing, usually negative
    Generic=8,//Used for anything else, not used by weapons
    True=9,//always does damage,used by tiles leaving the map and other things
}
public interface IDamageable{
    float dealDamage(float amount,DamageTypes type,Node2D source,Node2D projectile);//Returns the amount of damage actually dealt
}