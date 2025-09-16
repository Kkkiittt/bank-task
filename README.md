# bank-task
Used operators:
	select
	join
	group by
	having
	where
	order by
	sub-queries
	limit
	offset
Interesting cases:
	with optional filters, WHERE clause will be (IF clause OR param IS null)
	when retrieving top customers, grouping by id and name is required to retrive them both, although they are same
	with suspicious transactions, subquery is used to find id's of suspicious customers (by grouping and joining), then transactions joined with accounts of these customers from the last 10 minutes are retrieved
	when retrieving top customers, joining customers with OR clause is needed to get both incoming and outcoming transactions, as well as two joins for accounts (toAccount and fromAccount)