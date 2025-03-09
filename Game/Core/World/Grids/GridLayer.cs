public enum GridLayer{

	//Layer -1 UI
	ForegroundDecorationLayer=0,//rendered in front of entities
	CollisionLayer=1,//collides with entities
	InBetweenDecorationLayer=2,//directly behind entities
	InteractionLayer=3,//behind entities, can be interacted with
	ConnectionLayer=4,//directly behind interactive stuff, used for pipes and stuff
	BackgroundDecorationLayer=5,//used for decoration in front of the background
	BackgroundLayer=6,//backwalls and stuff
	BehindShipMapDecorationLayer=7,//Not used by ship, used by map decorations like trees
	BehindShipMapLayer=8,//Not used by ship, used by map decorations like huge cave walls

	//Layer 9 Map backgound Decoration images

	//Layer 10 main map brackground image
}