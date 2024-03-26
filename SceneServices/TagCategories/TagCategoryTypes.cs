namespace SceneServices.TagCategories
{
    public enum TagCategoryTypes
    {
        None = 0, // invalid
        Unknown,
        
        // From themes
        Author,
        Narrative,
        Attribute,
        Numeric, // Stim, Dom, Held, Love
        Service,
        Feeling, // Loving, Neutral, Rough, Aggressive
        Furniture,
        Contact, 
        ActorTypes,
        // additional
        AAFInformer,
        SAMEnhancedAnimations,
    }
}