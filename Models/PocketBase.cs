using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace MagicArchiver.Models;

public record struct LoginRecord {
  public string   id              { get; init; }
  public string   collectionId    { get; init; }
  public string   collectionName  { get; init; }
  public string   username        { get; init; }
  public bool     verfified       { get; init; }
  public bool     emailVisibility { get; init; }
  public string   email           { get; init; }
}

public record struct LoginResponse {
  public string      token  { get; init; }
  public LoginRecord record { get; init; }
}

public record struct Page<T> {
  public int page       { get; init; }
  public int perPage    { get; init; }
  public int totalPages { get; init; }
  public int totalItems { get; init; }
  public T[] items      { get; init; }
}


public record struct Collection {
  public int    id       { get; init; }
  public string uuid     { get; init; }
  public int    quantity { get; init; }
}

public record struct Card {
  public int      id                      { get; init; }
  public string   collectionId            { get; init; }
  public string   collectionName          { get; init; }
  public string?  artist                  { get; init; }
  public string?  asciiName               { get; init; }
  public string?  attractionLights        { get; init; }
  public string?  availability            { get; init; }
  public string?  boosterTypes            { get; init; }
  public string?  borderColor             { get; init; }
  public string?  cardKingdomEtchedId     { get; init; }
  public string?  cardKingdomFoilId       { get; init; }
  public string?  cardKingdomId           { get; init; }
  public string?  cardParts               { get; init; }
  public string?  cardsphereId            { get; init; }
  public string?  colorIdentity           { get; init; }
  public string?  colors                  { get; init; }
  public double?  convertedManaCost       { get; init; }
  public string?  defense                 { get; init; }
  public string?  duelDeck                { get; init; }
  public string?  faceName                { get; init; }
  public string?  finishes                { get; init; }
  public string?  flavorText              { get; init; }
  public string?  frameEffects            { get; init; }
  public string?  frameVersion            { get; init; }
  public int      hasAlternativeDeckLimit { get; init; }
  public int      hasFoil                 { get; init; }
  public int      hasNonFoil              { get; init; }
  public int      isAlternative           { get; init; }
  public int      isFullArt               { get; init; }
  public int      isFunny                 { get; init; }
  public int      isOnlineOnly            { get; init; }
  public int      isOversized             { get; init; }
  public int      isPromo                 { get; init; }
  public int      isRebalanced            { get; init; }
  public int      isReprint               { get; init; }
  public int      isReserved              { get; init; }
  public int      isStarter               { get; init; }
  public int      isStorySpotlight        { get; init; }
  public int      isTextless              { get; init; }
  public int      isTimeshifted           { get; init; }
  public string?  keywords                { get; init; }
  public string?  layout                  { get; init; }
  public string?  leadershipSkills        { get; init; }
  public string?  life                    { get; init; }
  public string?  loyalty                 { get; init; }
  public string?  manaCost                { get; init; }
  public double?  manaValue               { get; init; }
  public string?  mtgArenaId              { get; init; }
  public string?  mtgoFoilId              { get; init; }
  public string?  mtgoId                  { get; init; }
  public string?  name                    { get; init; }
  public string?  number                  { get; init; }
  public string?  originalReleaseDate     { get; init; }
  public string?  originalText            { get; init; }
  public string?  originalType            { get; init; }
  public string?  otherFaceIds            { get; init; }
  public string?  power                   { get; init; }
  public string?  printings               { get; init; }
  public string?  rarity                  { get; init; }
  public string?  rebalancedPrintings     { get; init; }
  public string?  setCode                 { get; init; }
  public string?  side                    { get; init; }
  public string?  subtypes                { get; init; }
  public string?  superTypes              { get; init; }
  public string?  text                    { get; init; }
  public string?  toughness               { get; init; }
  public string?  type                    { get; init; }
  public string?  types                   { get; init; }
  public string   uuid                    { get; init; }
  public string?  variations              { get; init; }
}

public record struct Token {
  public int     id            { get; init; }
  public string? colorIdentity { get; init; }
  public string? colors        { get; init; }
  public string? faceName      { get; init; }
  public string? finishes      { get; init; }
  public string? flavorText    { get; init; }
  public string? frameEffects  { get; init; }
  public string? frameVersion  { get; init; }
  public int     hasFoil       { get; init; }
  public int     hasNonFoil    { get; init; }
  public int     isFullArt     { get; init; }
  public int     isFunny       { get; init; }
  public int     isPromo       { get; init; }
  public int     isReprint     { get; init; }
  public int     isTextless    { get; init; }
  public string? keywords      { get; init; }
  public string? layout        { get; init; }
  public string? mtgArenaId    { get; init; }
  public string? name          { get; init; }
  public string? number        { get; init; }
  public string? orientation   { get; init; }
  public string? originalText  { get; init; }
  public string? originalType  { get; init; }
  public string? otherFaceIds  { get; init; }
  public string? power         { get; init; }
  public string? setCode       { get; init; }
  public string? side          { get; init; }
  public string? subtypes      { get; init; }
  public string? supertypes    { get; init; }
  public string? text          { get; init; }
  public string? toughness     { get; init; }
  public string? type          { get; init; }
  public string? types         { get; init; }
  public string  uuid          { get; init; }
}

public record struct Legality {
  public int     id     { get; init; }
  public string? format { get; init; }
  public string? status { get; init; }
  public string  uuid   { get; init; }
}

public record struct Set {
  public int       id               { get; init; }
  public int?      baseSetSize      { get; init; }
  public string?   block            { get; init; }
  public string?   booster          { get; init; }
  public int?      cardsphereSetId  { get; init; }
  public string    code             { get; init; }
  public int       isFoilOnly       { get; init; }
  public int       isForeignOnly    { get; init; }
  public int       isNonFoilOnly    { get; init; }
  public int       isOnlineOnly     { get; init; }
  public int       isPartialPreview { get; init; }
  public string?   keyruneCode      { get; init; }
  public string?   languages        { get; init; }
  public string?   mtgoCode         { get; init; }
  public string?   name             { get; init; }
  public string?   parentCode       { get; init; }
  public DateOnly? releaseDate      { get; init; }
  public string?   sealedProduct    { get; init; }
  public int?      tcgplayerGroupId { get; init; }
  public string?   tokenSetCode     { get; init; }
  public int?      totalSetSize     { get; init; }
  public string?   type             { get; init; }
}
