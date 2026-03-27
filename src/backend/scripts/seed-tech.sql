-- Technical seed for local development
-- Execute after migrations when required.

INSERT INTO users ("Id", "Email", created_at_utc, updated_at_utc)
VALUES ('11111111-1111-1111-1111-111111111111', 'seed@prevfinance.app', NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO profiles ("Id", "UserId", "FullName", created_at_utc, updated_at_utc)
VALUES ('22222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', 'Technical Seed User', NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO accounts ("Id", "UserId", "Name", "Type", "InitialBalance", created_at_utc, updated_at_utc)
VALUES ('33333333-3333-3333-3333-333333333333', '11111111-1111-1111-1111-111111111111', 'Conta Seed', 1, 1000.00, NOW() AT TIME ZONE 'UTC', NOW() AT TIME ZONE 'UTC')
ON CONFLICT ("Id") DO NOTHING;
